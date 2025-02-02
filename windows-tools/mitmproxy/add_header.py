import json
import os
from mitmproxy import http

# Load the configuration from config.json
def load_config():
    file_name = "config.json"  # Update if needed    
    # Get the absolute path of the directory containing this script
    script_dir = os.path.dirname(os.path.realpath(__file__))
    config_file = os.path.join(script_dir, file_name)  # Full path to config.json
    
    if not os.path.exists(config_file):
        print(f"Error: The file {config_file} does not exist.")
        return None
    
    with open(config_file, 'r') as f:
        config = json.load(f)
    
    return config

# Get the list of hosts and headers from config.json
def get_hosts(config):
    return config.get("hosts", [])

def get_headers(config):
    return config.get("headers", {})

# Get the value of match_subdomains from config.json
def should_match_subdomains(config):
    return config.get("match_subdomains", False)

# Function to add headers to request or response based on configuration
def add_headers(flow, add_to_response=False, headers={}):
    for key, value in headers.items():
        if add_to_response:
            flow.response.headers[key] = value
        else:
            flow.request.headers[key] = value

# Check if the host is a subdomain of any host in the list
def is_matching_host(host, hosts, match_subdomains=False):
    for configured_host in hosts:
        if match_subdomains:
            if host.endswith(configured_host):  # Check for subdomain
                return True
        else:
            if host == configured_host:  # Check for exact match
                return True
    return False

# Handle the request (request) or response (response)
def request(flow: http.HTTPFlow):
    # Load the configuration from config.json
    config = load_config()
    if config is None:
        return

    # Get the list of hosts from config.json
    hosts = get_hosts(config)
    
    # Get the headers from config.json
    headers = get_headers(config)
    
    # Check if the host matches and if headers should be added to the request
    match_subdomains = should_match_subdomains(config)
    if is_matching_host(flow.request.pretty_host, hosts, match_subdomains):
        print(f"Adding headers to request for {flow.request.pretty_host}")
        if config.get("add_to_request", False):
            add_headers(flow, add_to_response=False, headers=headers)

def response(flow: http.HTTPFlow):
    # Load the configuration from config.json
    config = load_config()
    if config is None:
        return

    # Get the list of hosts from config.json
    hosts = get_hosts(config)
    
    # Get the headers from config.json
    headers = get_headers(config)
    
    # Check if the host matches and if headers should be added to the response
    match_subdomains = should_match_subdomains(config)
    if is_matching_host(flow.request.pretty_host, hosts, match_subdomains):
        print(f"Adding headers to response for {flow.request.pretty_host}")
        if config.get("add_to_response", False):
            add_headers(flow, add_to_response=True, headers=headers)
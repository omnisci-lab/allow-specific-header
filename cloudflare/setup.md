### Setup

Steps:
 - Go to Cloudflare Dashboard → Select your domain.
 - Navigate to WAF -> Custom Rules
 - Create a new rule:
    - 1. Field: "Header"; Name: "x-special-code"; Operator: does not equal; Value: "your-special-code"
    - And 
    - 2. Field: Hostname; Operator: equal; Value: "your-host"
 - Save & Deploy.


### Expression
```(all(http.request.headers["x-special-code"][*] ne "your-special-code") and http.host eq "your-host")```
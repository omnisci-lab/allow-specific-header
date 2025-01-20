chrome.runtime.onInstalled.addListener(() => {
  fetch(chrome.runtime.getURL('config.json'))
    .then(response => response.json())
    .then(config => {
      const code = config.code || 'default-code'; // Giá trị mặc định nếu không tìm thấy
	  
      // Định nghĩa rule để thêm custom header vào tài nguyên hình ảnh và video
      const rule = {
        "id": 1,
		"priority": 1,
        "action": {
          "type": "modifyHeaders",
          "requestHeaders": [
            {
              "header": "x-special-code",
              "operation": "set",
              "value": code  // Sử dụng giá trị từ file cấu hình
            }
          ]
        },
        "condition": {
          "urlFilter": "*://*.example.com/*",
          "resourceTypes": ["main_frame", "xmlhttprequest", "script", "stylesheet", "image", "media", "other"]
        }
      };

      // Cập nhật rule
      chrome.declarativeNetRequest.updateDynamicRules({
        addRules: [rule],
		removeRuleIds: [1]
      });
    })
    .catch(error => {
      console.error('Error loading config:', error);
    });
});
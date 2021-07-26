curl -v -X POST \
-H 'Cache-Control: no-cache' \
-H "Content-type: application/json" \
-d @data.json \
https://<REPLACE_WITH-YOUR-FUNCTION-RESOURCE-NAME>.azurewebsites.net/api/sendemail?code=<REPLACE_WITH-YOUR-FUNCTION-RESOURCE-FUNCTION-KEY>
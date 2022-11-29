param(
$sessioncode = "XXXXXX-XXXXXX",
$targetFolder = "D:\scratch\url-exporter",
$binFolder = "..\..\AskDelphi.Tools.TopicExporter\bin\Debug\net5.0"
)

New-Item -ItemType Directory -Force "$targetFolder" | Out-Null

& "$binFolder\AskDelphi.Tools.TopicExporter.exe" `
    --session-code "$sessioncode" `
    --config ".\url-exporter.json" `
    --tenantid "82716c4e-7748-4b11-803c-5511c0e5c933" `
    --hostid "ee8b936e-9fad-45f7-a901-0bbb8788dc60" `
    --projectid "051bef14-6c7c-4a80-bb5f-e934a6640a3c" `
    --aclid "0618173e-3441-4e92-9ffd-67890eaa8ec8" `
    --out "$targetFolder\export-urls-1.xlsx" `
    --save-auth-file "$targetFolder\auth.json"
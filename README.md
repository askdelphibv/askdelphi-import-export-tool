# Topic Importer/Exporter

These tools allow exporting parts of an AskDelphi projects into Excel or JSON files. The data in these files can be edited after which the updated content can be re-uploaded to perform a bulk update of the content.

## Required set-up

### Enable authenticating with a portal code
Enable the new mobile experience in at least one publication for the project. Publish this to a hosting environment that you wish to use for authentication.
Enable QR code login in the publication options.
There is no need to set up 

### Set up an ACL
Set up a CMS-level ACL (Regional Editing) as follows:
1. Deny ALL
2. Allow claim type and value for user from /user/claims for authentication publication
3. Allow claim type and value for user from /user/claims for authentication publication
etc.

In the new-style ACL rules, assign a role with topic read and write access without any further restrictions; In the old-style rules, be sure not to add any required hierarchy node IDs.

### Note down the following information
1. Tenant Guid 
2. Hosting Environment Guid for your authentication publication
3. Project Guid of the project you would like to import/export
4. The ACL ID for the ACL that you created
5. A session code that you obtained via the QR code login panel on your publication. You should obvtain this last-minute since these codes are only valid for a short time. Also be aware that these codes can be used no more than once. When not using the ```--save-auth``` option, you  must request a new session code for every run.

### Topic configuration file
Obtain or create a topic configuration file. This file is a JSON file that's 'structured as follows:

```json
{
    "Namespace": "http://tempuri.org/doppio-external",
    "TopicTypes": [],
  "Mappings": [
    {
      "JSONPath": "$.Groups[?(@.PartGroupId=='basic-data')].Parts[?(@.PartId=='title-description')].Editors[?(@.EditorFieldId=='title')].Value.String.Value",
      "TargetField": "title"
    },
    {
      "JSONPath": "$.Groups[?(@.PartGroupId=='basic-data')].Parts[?(@.PartId=='title-description')].Editors[?(@.EditorFieldId=='title')].Value.String.Markup",
      "TargetField": "titleMarkup"
    },
    {
      "JSONPath": "$.Groups[?(@.PartGroupId=='content')].Parts[?(@.PartId=='link-meta-data')].Editors[?(@.EditorFieldId=='url')].Value.String.Value",
      "TargetField": "url"
    }
  ]
}
```

The Namespace must be a single AskDelphi topic type namespace for which we're going to export content.
The TopicTypes array is optional. If non-empty you can specify additional topic type guids here to restrict the xport to only topics of that specific type.
The Mappings is an Array of:
- *JSONPath*: A JSON path in the part definition output. 
- *TargetField*: A valid property name or column name to write the output to. If the output is in JSON format, then this must be a valid JSON name.

The above sample exports the title, titleMarkup and the URL editor field value for all external content topics, and can be used to re-import updated title, title markup and url.

TopicGuid will always be added.

#### Obtaining JSON paths

To obtain sample JSON data for your JSON paths, you can use the built-in path tester for the export tool. You can add a command-line option: ```-test-jsonpath "$"``` which will export the full JSON for each topic with matching namespace and type.

To test a single JSON path against all these topics, use ```-test-jsonpath "YOUR JSON PATH"``` to see what the output would be. When developing a JSON path, just grab a single JSON document from the "$" export, and use a tool like https://jsonpath.com/ to develop your JSON path.

#### JSONPath links:
- https://goessner.net/articles/JsonPath/ (https://tools.ietf.org/id/draft-goessner-dispatch-jsonpath-00.html)
- https://www.baeldung.com/guide-to-jayway-jsonpath
- https://jsonpath.com/

## Exporting

The following command-line options are available for the AskDelphi.Tools.TopicExporter.exe program:

|Option|Argument|Explanation|
|------|--------|-----------|
|-s, --session-code|{session-code}|Required. Single-use session code. You can obtain this using the mobile QR code feature in the publication. These codes are single-use so a new code is required for every run.|
|-t, --tenantid|{tenant guid}|Required. The tenant guid for the tenant in whose context the operation is carried out. This can be obtained from the hosting environment configuration editor, or from AskDelphi support.|
|-h, --hostid|{hosting environment guid}|Required. The hosting environment guid for the tenant in whose context the operation is carried out. This can be obtained from the hosting environment configuration editor, or from AskDelphi support.|
|-a, --aclid|{ACL entry ID}|Required. Identifier of the ACL that grants editing access to the user for whom the session code was created. You can get this from the ACL editor for the AskDelphi project.|
|-p, --projectid|{project guid}|Required. Identifies the project for which the tool needs to be run, copy this from the project properties view.|
|-c, --config|{path to the topic-configuration file}|Required. Specially crafted mapping file that describes which information from the topic should be written into the output where.|
|-o, --out|{path to the output}|Required. Output filename. If the filename ends in .xlsx an Excel fiel will be created, if the name ends in .json a JSON file will be created.|
|-x, --api|{api url}|(Default: https://askdelphi-prod-editing-api.azurewebsites.net/) Full URL to the editing API server that is to be used, defaults to the production server, specify only when testing with other environments such as staging or test.|
|-b, --save-auth-file|{path to auth file}|Authentication data file name. If specified login information is stored here between sessions and is re-used.|
|-j, --test-jsonpath|{JSON path to test}|If specified, writes the result of the JSON path specified as argument to this option to the console for each matching entry, does not generate any output file.|

About the ```--save-auth-file``` file option: When you use this, then the token and refresh token obtained from the session-code are stored in that file. The next time you start the tool with the same option, the token from that file is refreshed and the session code parameter is not required.

So, basically the first time you se the tool for a specific environment, you specify both the --session-code and the --save-auth option. On consecutive runes you only specify the --save-auth option. You can use the same file for import and export.

### Sample command-line

```powershell
AskDelphi.Tools.TopicExporter.exe `
  --tenantid "01234567-7748-4b11-803c-5511c0e5c933" `
  --hostid "01234567-9fad-45f7-a901-0bbb8788dc60" `
  --projectid "01234567-6c7c-4a80-bb5f-e934a6640a3c" `
  --aclid "01234567-3441-4e92-9ffd-67890eaa8ec8" `
  --config "D:\git\askdelphi-tools-topic-import-export\Samples\URL Exporter\url-exporter.json"  `
  --save-auth-file "d:\scratch\url-exporter\auth.json" `
  --out "d:\scratch\url-exporter\export-urls-1.json" `
  --session-code "SESSION-CODE"
```

## Building and debugging
You can just build either "AskDelphi.Tools.TopicExporter" or "AskDelphi.Tools.TopicImporter" in debug mode to test it. The release versiosn are built on TFS


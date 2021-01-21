function InvokeDownload(Primarycontrol)
{
debugger;
var formContext= Primarycontrol;
var RecordUniqueName=GetUniqueName(formContext);
	 var data = ExecuteAction(formContext);
        if(data == '')
            return;
        
        JSONToCSVConvertor(data.AuditHistories, "Audit History", true,RecordUniqueName);
}
function GetUniqueName(formContext)
{
	var name= !formContext.getAttribute("name").getValue()?formContext.data.entity.getId():formContext.getAttribute("name").getValue();
	return name;
}
function ExecuteAction(formContext)
{
//debugger;
var finalOutput={};
var parameters = {};
const recordId=formContext.data.entity.getId();
const recordLogicalName=formContext.data.entity.getEntityName();;
parameters.TargetId = recordId;
parameters.EntityLogicalName = recordLogicalName;

var req = new XMLHttpRequest();
req.open("POST", Xrm.Page.context.getClientUrl() + "/api/data/v9.1/dev_ReturnJson", false);
req.setRequestHeader("OData-MaxVersion", "4.0");
req.setRequestHeader("OData-Version", "4.0");
req.setRequestHeader("Accept", "application/json");
req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
req.onreadystatechange = function() {
    if (this.readyState === 4) {
        req.onreadystatechange = null;
        if (this.status === 200) {
            var results = JSON.parse(this.response);
            finalOutput=JSON.parse(results.JsonResponse);
        } else {
            Xrm.Utility.alertDialog(this.statusText);
        }
    }
};
req.send(JSON.stringify(parameters));
return finalOutput;
}
function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel,RecordUniqueName) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    
    var CSV = '';    
    //Set Report title in first row or line
    
    CSV += ReportTitle + '\r\n\n';

    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";
        
        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {
            
            //Now convert each value to string and comma-seprated
            row += index + ',';
        }

        row = row.slice(0, -1);
        
        //append Label row with line break
        CSV += row + '\r\n';
    }
    
    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";
        
        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }

        row.slice(0, row.length - 1);
        
        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {        
        alert("Invalid data");
        return;
    }   
    
    //Generate a file name
    var fileName = "AuditReport_";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += RecordUniqueName.replace("{", "").replace("}","");   
    
    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);
    
    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    
    
    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");    
    link.href = uri;
    
    //set the visibility hidden so it will not effect on your web-layout
    //link.style = "visibility:hidden";
    link.download = fileName + ".csv";
    
    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
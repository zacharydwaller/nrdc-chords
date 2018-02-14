var serviceUrl = "http://localhost:3485/";
var chordsUrl = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com/";

var selectedNetwork = "NevCAN";
var selectedSite = 0;
var selectedSystem = 0;
var selectedDeployment = 0;
var selectedStreams = new Set();

var hiClasses = "list-group-item hierarchy-item";

var loader = "<img id=\"loading\" src=\"img/spinner.gif\" style=\"width:100px\" />";

var systemsHeader;
var deploymentsHeader;
var streamsHeader;
var selectedStreamsHeader;

var startCalendar;
var endCalendar;

var sessionKey;
var intervalFunc;

$(document).ready(function ()
{
    $(".hierarchy-item").remove();

    // Attach netbuttonClick function to network buttons 
    $(".net-button").click(netbuttonClick);
    
    getHeaders();
    hideHeaders();

    // Retrieve NevCAN sites
    expandHierarchy(serviceUrl +"DataCenter/" + selectedNetwork + "/sites?", expandSites);

    $("#VisResult").hide();

    startCalendar = $("#startdate");
    endCalendar = $("#enddate");

    startCalendar.datepicker();
    endCalendar.datepicker();
});

function netbuttonClick()
{
    selectedNetwork = $(this).attr("value");

    $(".net-button").removeClass("active");
    $(this).addClass("active");

    $(".hierarchy-item").remove();

    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/sites?", expandSites);

    $("#streamTab").click();
}

function siteButtonClick()
{
    selectedSite = $(this).attr("value");

    $(".site-button").removeClass("active");
    $(this).addClass("active");

    $(".system-button").remove();
    $(".deployment-button").remove();
    $(".stream-button").remove();

    $("#system-list").append(loader);
    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/systems?siteID=" + selectedSite, expandSystems);
}

function systemButtonClick()
{
    selectedSystem = $(this).attr("value");

    $(".system-button").removeClass("active");
    $(this).addClass("active");

    $(".deployment-button").remove();
    $(".stream-button").remove();

    $("#deployment-list").append(loader);
    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/deployments?systemID=" + selectedSystem, expandDeployments);
}

function deploymentButtonClick()
{
    selectedDeployment = $(this).attr("value");

    $(".deployment-button").removeClass("active");
    $(this).addClass("active");

    $(".stream-button").remove();

    $("#stream-list").append(loader);
    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/streams?deploymentID=" + selectedDeployment, expandStreams);
}

function streamButtonClick()
{
    var id = $(this).attr("value");

    if (selectedStreams.has(id))
    {
        selectedStreams.delete(id);
        $(this).removeClass("active");
    }
    else
    {
        selectedStreams.add(id);
        $(this).addClass("active");
    }

    updateSelectedStreams();
}

function selstreamButtonClick()
{
    var id = $(this).attr("value");

    selectedStreams.delete(id);

    $(".stream-button").filter(function () { return $(this).attr("value") == id }).removeClass("active");
    updateSelectedStreams();
}

function visualizeButtonClick()
{
    uri = serviceUrl + "Session/newSession?"
        + "netAlias=" + selectedNetwork;

    for (let id of selectedStreams)
    {
        uri += "&streamIDs=" + id;
    }

    var start = startCalendar.datepicker("getDate");
    var end = endCalendar.datepicker("getDate");
    if (start != null)
    {
        var date = new Date(start);

        uri += "&startTime=" + date.toISOString(); 
    }

    if (end != null)
    {
        var date = new Date(end);

        uri += "&endTime=" + date.toISOString();
    }

    $("#VisOptions").hide();

    $("#VisResult").show();
    $("#VisResult").append(loader);

    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result)
        {
            $("#loading").remove();

            if (result.Success)
            {
                sessionKey = result.Data;
                $("#sessionKey").append("Your Session Key: " + sessionKey);
                window.open(chordsUrl, "_blank");

                refreshSession();

                interval = $("#refreshdelay").val() * 1000;
                intervalFunc = setInterval(refreshSession, interval);

                console.log(interval);
            }
            else
            {
                //$("#sessionKey").append(result.Message);
                console.error(result.Message);

                $("#VisOptions").show();
                $("#VisResult").hide();
            }
        },

        error: function ()
        {
            $("#loading").remove();

            console.error("Call to " + uri + " unable to be completed.");

            $("#VisResult").append("Call to " + uri + " unable to be completed.");
        }
    });
}

function resumeButtonClick()
{
    sessionKey = $("#keyInput").val();
    interval = $("#refreshdelay2").val() * 1000;

    $("#VisOptions").hide();

    $("#VisResult").show();
    

    refreshSession();
    intervalFunc = setInterval(refreshSession, interval);

    $("#sessionKey").append("Resuming Session: " + sessionKey);

    window.open(chordsUrl, "_blank");
}

function refreshSession()
{
    uri = serviceUrl + "Session/refreshSession?"
        + "key=" + sessionKey;

    console.log("Refreshed");

    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result)
        {
            if (!result.success)
            {
                $("#sessionKey").append("<br>" + result.Message);
                clearInterval(intervalFunc);
            }
        },

        error: function ()
        {
            console.error("Call to " + uri + " unable to be completed.");
            console.error(result.Message);

            $("#VisResult").append("Error: Call to " + uri + " unable to be completed.");
            $("#VisResult").append(result.Message);
        }
    });
}

function updateSelectedStreams()
{
    if (selectedStreams.size > 0)
    {
        selectedStreamsHeader.show();
    }
    else
    {
        selectedStreamsHeader.hide();
    }

    expandSelectedStreams();
}

function fadeInButton(button)
{
    $(button).hide();
    $(button).fadeIn();
}

function createButton(buttonClass, listName, callback)
{
    var item = document.createElement("a");

    $(item).addClass(hiClasses);
    $(item).addClass(buttonClass);
    $(item).click(callback);
    $(listName).append(item);

    fadeInButton(item);

    return item;
}

function expandHierarchy(uri, callback)
{
    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result)
        {
            $("#loading").remove();

            //console.log(result);
            if (result.Success == true)
            {
                callback(result.Data);
            }
            else
            {
                console.error(result.Message);
            }
        },

        error: function ()
        {
            $("#loading").remove();

            console.error("Call to " + uri + " unable to be completed.");
        }
    });
}

function expandSites(data)
{
    hideHeaders();

    for (var i = 0; i < data.length; i++)
    {
        var button = createButton("site-button", "#site-list", siteButtonClick);
        button.innerHTML = data[i].Alias;
        $(button).attr("value", data[i].ID)
    }
}

function expandSystems(data)
{
    systemsHeader.show();
    deploymentsHeader.hide();
    streamsHeader.hide();

    for (var i = 0; i < data.length; i++)
    {
        var button = createButton("system-button", "#system-list", systemButtonClick);
        button.innerHTML = data[i].Name;
        $(button).attr("value", data[i].ID)
    }
}

function expandDeployments(data)
{
    deploymentsHeader.show();
    streamsHeader.hide();

    for (var i = 0; i < data.length; i++)
    {
        var button = createButton("deployment-button", "#deployment-list", deploymentButtonClick);
        button.innerHTML = data[i].Name;
        $(button).attr("value", data[i].ID)
    }
}

function expandStreams(data)
{
    streamsHeader.show();

    for (var i = 0; i < data.length; i++)
    {
        var button = createButton("stream-button", "#stream-list", streamButtonClick);
        button.innerHTML =
            "<h4 class=\"list-group-item-heading\">" + data[i].ID + " - " + data[i].Property.Name + "</h4>" +
            "<p class=\"list-group-item-heading\">" + data[i].DataType.Name + "</p>" +
            "<p class=\"list-group-item-text\">" + "(" + data[i].Units.Abbreviation + ")" + "</p>" +
            "<p class=\"list-group-item-text\">" + "Interval: " + data[i].MeasurementInterval + "</p>";
        $(button).attr("value", data[i].ID)

        if (selectedStreams.has(data[i].ID.toString()))
        {
            $(button).addClass("active");
        }
    }
}

function expandSelectedStreams()
{
    $(".selstream-button").remove();

    for (let id of selectedStreams)
    {
        var button = createButton("selstream-button", "#selstream-list", selstreamButtonClick);
        button.innerHTML = id;
        $(button).attr("value", id)
    }
}

function getHeaders()
{
    systemsHeader = $("#systems-header");
    deploymentsHeader = $("#deployments-header");
    streamsHeader = $("#streams-header");
    selectedStreamsHeader = $("#selectedstreams-header");
}

function hideHeaders()
{
    systemsHeader.hide();
    deploymentsHeader.hide();
    streamsHeader.hide();
    selectedStreamsHeader.hide();
}
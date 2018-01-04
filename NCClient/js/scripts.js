var serviceUrl = "http://localhost:3485/DataCenter/";

var selectedNetwork = "";
var selectedSite = 0;
var selectedSystem = 0;
var selectedDeployment = 0;
var selectedStream = 0;

var hiClasses = "list-group-item hierarchy-item";

var loader = "<img id=\"loading\" src=\"img/spinner.gif\" style=\"width:100px\" />";

$(document).ready(function ()
{
    $(".hierarchy-item").remove();

    // Attach netbuttonClick function to network buttons 
    $(".net-button").click(netbuttonClick);
});

function netbuttonClick()
{
    selectedNetwork = $(this).attr("value");

    $(".net-button").removeClass("active");
    $(this).addClass("active");

    $(".hierarchy-item").remove();

    expandHierarchy(serviceUrl + selectedNetwork + "/sites?", expandSites);
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
    expandHierarchy(serviceUrl + selectedNetwork + "/systems?siteID=" + selectedSite, expandSystems);
}

function systemButtonClick()
{
    selectedSystem = $(this).attr("value");

    $(".system-button").removeClass("active");
    $(this).addClass("active");

    $(".deployment-button").remove();
    $(".stream-button").remove();

    $("#deployment-list").append(loader);
    expandHierarchy(serviceUrl + selectedNetwork + "/deployments?systemID=" + selectedSystem, expandDeployments);
}

function deploymentButtonClick()
{
    selectedDeployment = $(this).attr("value");

    $(".deployment-button").removeClass("active");
    $(this).addClass("active");

    $(".stream-button").remove();

    $("#stream-list").append(loader);
    expandHierarchy(serviceUrl + selectedNetwork + "/streams?deploymentID=" + selectedDeployment, expandStreams);
}

function streamButtonClick()
{
    selectedStream = $(this).attr("value");

    $(".stream-button").removeClass("active");
    $(this).addClass("active");
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

function createCollapsible(text)
{
    var panel = document.createElement("div");
    $(panel).addClass("panel panel-default");

    var heading = document.createElement("div");
    $(heading).addClass("panel-heading");
    $(panel).append(heading);

    var title = document.createElement("h4");
    $(title).addClass("panel-title");
    $(heading).append(title);

    var dataToggle = document.createElement("a");
    $(dataToggle).attr("data-toggle", "collapse");
    $(dataToggle).attr("href", "#" + text);
    dataToggle.innerHTML = text;
    $(title).append(dataToggle);

    var collaspe = document.createElement("div");
    $(collapse).addClass("panel-collapse collapse");
    $(collapse).attr("id", text);

    // Unfinished
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

            console.log(result);
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
    for (var i = 0; i < data.length; i++)
    {
        var button = createButton("site-button", "#site-list", siteButtonClick);
        button.innerHTML = data[i].Alias;
        $(button).attr("value", data[i].ID)
    }
}

function expandSystems(data)
{
    for (var i = 0; i < data.length; i++)
    {
        var button = createButton("system-button", "#system-list", systemButtonClick);
        button.innerHTML = data[i].Name;
        $(button).attr("value", data[i].ID)
    }
}

function expandDeployments(data)
{
    for (var i = 0; i < data.length; i++)
    {
        var button = createButton("deployment-button", "#deployment-list", deploymentButtonClick);
        button.innerHTML = data[i].Name;
        $(button).attr("value", data[i].ID)
    }
}

function expandStreams(data)
{
    for (var i = 0; i < data.length; i++)
    {
        var button = createButton("stream-button", "#stream-list", streamButtonClick);
        button.innerHTML =
            "<h4 class=\"list-group-item-heading\">" + data[i].DataType.Name + "</h4>" +
            "<p class=\"list-group-item-text\">" + "Interval: " + data[i].MeasurementInterval + "</p>";
        $(button).attr("value", data[i].ID)
    }
}
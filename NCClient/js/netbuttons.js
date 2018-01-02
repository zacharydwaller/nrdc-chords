var nevadaBlue = "rgb(0,46,98)";
var aliceBlueTransp = "rgba(240,248,255,0.75)";
var nevadaBlueTransp = "rgba(0,46,98,0.75)";

function resetTabContents()
{
    var contents = document.getElementsByClassName("tab-content");

    // Close tabs
    var i;
    for (i = 0; i < contents.length; i++)
    {
        contents[i].style.display = "none";
    }
}

function resetTabs()
{
    var tabs = document.getElementsByClassName("tab");
    var i;

    for (i = 0; i < tabs.length; i++)
    {
        tabs[i].style.color = nevadaBlue;
        tabs[i].style.backgroundColor = aliceBlueTransp;
    }
}

function closeTab()
{
    resetTabs();
    resetTabContents();
}

function openTab(tabName, tabContentName)
{
    resetTabs();
    resetTabContents();

    // Color selected tab
    var tab = document.getElementById(tabName);
    tab.style.color = "white";
    tab.style.backgroundColor = nevadaBlueTransp;

    // Open tab content
    var tabContent = document.getElementById(tabContentName);
    tabContent.style.display = "block";
}
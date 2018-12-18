let gameofcodesShare = +($('#gameofcodes1').text())
let tuttifruttiShare = +($('#tuttifrutti1').text())
let treasuresofegyptShare = +($('#treasuresofegypt1').text())
let total = gameofcodesShare + tuttifruttiShare + treasuresofegyptShare

function createChart() {
    $("#chart").kendoChart({
        theme: "moonLight",
        title: {
            position: "top",
            text: "Games by popularity"
        },
        legend: {
            visible: true
        },
        chartArea: {
            background: ""
        },
        seriesDefaults: {
            labels: {
                visible: true,
                background: "transparent",
                template: "#= category #: \n #= value#%"
            }
        },
        series: [{
            type: "pie",
            startAngle: 150,
            data: [{
                category: "Game of Codes",
                value: ((gameofcodesShare / total) * 100).toFixed(2),
                color: "#9de219"
            }, {
                    category: "Tutti Frutti",
                    value: ((tuttifruttiShare / total) * 100).toFixed(2),
                color: "#068c35"
            }, {
                    category: "Treasures of Egypt",
                    value: ((treasuresofegyptShare / total) * 100).toFixed(2),
                color: "#033939"
            }]
        }],
        tooltip: {
            visible: true,
            format: "{0}%"
        }
    });
}

$(document).ready(createChart);
$(document).bind("kendo:skinChange", createChart);
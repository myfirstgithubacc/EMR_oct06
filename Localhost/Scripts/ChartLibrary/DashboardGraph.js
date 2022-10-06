function DrawChart(chartTypeName, labels, dataSet, chartTitle, divId) {
    //alert("Test 4");
    var dataT = {
        labels: labels,
        datasets: [{
            label: "Reading",
            data: dataSet,
            fill: false,
            backgroundColor: ["rgba(54, 162, 235, 0.2)", "rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"],
            //borderColor: ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"],  
            borderColor: '#ff8800',
            borderWidth: 2,

        }]
    };
    var myNewChart = new Chart(divId, {
        type: chartTypeName,
        data: dataT,
        options: {
            responsive: true,
            legend: {
                display: false
            },
            scales: {  
                xAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: false, labelString: '' }, ticks: { beginAtZero: true } }],
                yAxes: [{ gridLines: { display: true }, display: true, scaleLabel: { display: false, labelString: '' }, ticks: { beginAtZero: true } }],
            },
            title: { display: true, text: chartTitle, fontSize: 16 },
            plugins: {
                datalabels: {
                    align: function (context) {
                        var index = context.dataIndex;
                        var curr = context.dataset.data[index];
                        var prev = context.dataset.data[index - 1];
                        var next = context.dataset.data[index + 1];
                        return prev < curr && next < curr ? 'end' :
                            prev > curr && next > curr ? 'end' :
                            'center';
                    },

                    backgroundColor: 'rgba(255, 255, 255, 0.7)',
                    borderColor: 'rgba(128, 128, 128, 0.7)',
                    borderRadius: 100,
                    borderWidth: 1,
                    color: function (context) {
                        var i = context.dataIndex;
                        var value = context.dataset.data[i];
                        var prev = context.dataset.data[i - 1];
                        var diff = prev !== undefined ? value - prev : 0;
                        return diff < 0 ? Samples.color(10) :
                            diff > 0 ? Samples.color(10) :
                            Samples.color(10);
                    },
                    font: {
                        size: 11,
                        weight: 600
                    },
                    offset: 8,
                    formatter: function (value, context) {
                        //var i = context.dataIndex;
                        //var prev = context.dataset.data[i - 1];
                        //var diff = prev !== undefined ? prev - value : 0;
                        //var glyph = diff < 0 ? '\u25B2' : diff > 0 ? '\u25BC' : '\u25C6';
                        //return glyph + ' ' + Math.round(value);
                    },

                }
            }
        }
    });
}
// Bar chart

if ($('#barChart').length) {

    var ctx = document.getElementById("barChart");
    var mybarChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [2022, 2023],
            datasets: [{
                label: '# étudiants',
                backgroundColor: "#26B99A",
                data: [1, 2]
            }]
        },

        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });

}
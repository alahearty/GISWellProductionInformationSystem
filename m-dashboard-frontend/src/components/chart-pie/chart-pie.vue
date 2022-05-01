<script lang='ts'>
import {Component, Prop, Mixins, Watch} from 'vue-property-decorator';
import { Pie } from 'vue-chartjs';

import { ChartData } from 'chart.js';

@Component
export default class ChartPie extends Mixins(Pie) {
    @Prop({ default: () => [] }) chartData!: ChartData;
    private colors: string[] = ['#3c1bc2', '#1bc245'];

    @Watch('chartData')
    onChartDataChange() {
        this.generateChart();
    }

    private generateChart() {
        const options = {
            cutoutPercentage: 50,
            // legend: {
            //     display: true,
            // },
            // title: {
            //     display: true
            // },
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                yAxes: [{
                        display: false,
                        position: 'left',
                    },
                ],
                xAxes: [{
                    display: false,
                    position: 'left',
                }],
            },
        };
        this.renderChart(this.chartData, options);
    }

    mounted() {
        this.generateChart();
    }
}
</script>
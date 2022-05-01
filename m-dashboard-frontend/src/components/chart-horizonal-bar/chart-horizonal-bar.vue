<script lang='ts'>
import {Component, Prop, Mixins, Watch} from 'vue-property-decorator';
import { HorizontalBar } from 'vue-chartjs';

import { ChartData } from 'chart.js';

@Component
export default class HorizontalBarChart extends Mixins(HorizontalBar) {
    @Prop({ default: () => [] }) chartData!: ChartData;
    @Prop({ default: false }) showLegend!: boolean;
    // private colors: string[] = ['#3c1bc2', '#1bc245'];

    @Watch('chartData')
    onChartDataChange() {
        this.generateChart();
    }

    private generateChart() {
        const options = {
            plugins: {
                datalabels: {
                    // @ts-ignore
                    labels: {
                        value: {
                            color: '#262626'
                        }
                    },
                    formatter: function(value: number) {
                        return value.toLocaleString()
                    },
                    anchor: 'end',
                    clamp: true,
                    align: 'end',
                    offset: -10
                },
            },
            legend: {
                display: this.showLegend
            },
            title: {
                display: false
            },
            // responsive: true,
            // maintainAspectRatio: true,
            aspectRatio: 1,
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                yAxes: [{
                        display: true,
                        position: 'left',
                    },
                ],
                xAxes: [{
                    display: true,
                    position: 'left',
                }],
            },
        };
        // @ts-ignore
        this.renderChart(this.chartData, options);
    }

    mounted() {
        this.generateChart();
    }
}
</script>
<script lang="ts">
import Component, {
    mixins,
} from 'vue-class-component';
import {
    Bar
} from 'vue-chartjs';
import {
    Prop,
    Watch,
} from 'vue-property-decorator';
import { ChartData } from '@/types/Utility';

@Component
export default class ChartMultiLine extends mixins(Bar) {
    // @Prop() yAxes!: any[];
    // @Prop() xAxisTitle!: string;
    @Prop({ default: () => [] }) chartData!: ChartData;

    @Watch('chartData')
    onChartDataChange() {
        this.generateChart();
    }

    private generateChart() {
        const options = {
            // legend: {
            //     display: false,
            // },
            title: {
                display: false
            },
            aspectRatio: 1,
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                yAxes: [
                    {
                        display: false,
                        position: 'left',
                    },
                    {
                        type: 'linear',
                        display: true,
                        position: 'left',
                        id: 'Oil Rate',
                        scaleLabel: {
                            display: true,
                            labelString: 'Oil Rate (stb/day)',
                            fontColor: '#c45850',
                        },
                        ticks: {
                            beginAtZero: true,
                            fontColor: '#c45850',
                        },
                        gridLines: {
                            drawOnChartArea: true,
                            fontColor: '#c45850',
                        },
                    },
                    {
                        type: 'linear',
                        display: true,
                        position: 'left',
                        id: 'GOR',
                        scaleLabel: {
                            display: true,
                            labelString: 'GOR (scf/stb)',
                            fontColor: 'green',
                        },
                        ticks: {
                            beginAtZero: true,
                            fontColor: 'green',
                        },
                        gridLines: {
                            drawOnChartArea: false,
                            color: 'green',
                            fontColor: 'green',
                        },
                    },
                    {
                        type: 'linear',
                        display: true,
                        position: 'right',
                        id: 'Water Cut',
                        scaleLabel: {
                            display: true,
                            labelString: 'Water Cut (%)',
                            fontColor: 'steelblue',
                        },
                        ticks: {
                            beginAtZero: true,
                            fontColor: 'steelblue',
                            suggestedMin: 0,
                            suggestedMax: 100,
                        },
                        gridLines: {
                            drawOnChartArea: false,
                            color: 'steelblue',
                            fontColor: 'steelblue',
                        },
                    },
                    {
                        type: 'linear',
                        display: true,
                        position: 'right',
                        id: 'CUM',
                        scaleLabel: {
                            display: true,
                            labelString: 'Cummulative Oil Produced (MMStb)',
                            fontColor: 'red',
                        },
                        ticks: {
                            beginAtZero: true,
                            fontColor: 'red',
                        },
                        gridLines: {
                            drawOnChartArea: false,
                            color: 'red',
                            fontColor: 'red',
                        },
                    },
                ],
                xAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: 'Date',
                        // fontColor: 'steelblue',
                    },
                    type: 'time',
                    unit: 'year',
                    display: true,
                }],
            },
            plugins: [],
        };
        this.renderChart(this.chartData, options);
    }

    mounted() {
        this.generateChart();
    }
}
</script>

<template>
    <section>
        <b-container>
            <b-row>
                <b-col md="12">
                    <h3 class="text-center">PECON FIELD SURVEILLANCE</h3>
                </b-col>
                <b-col md="2">
                    <WellsSelector @wellsChange="getWellsData($event)"/>
                </b-col>
                <b-col md="6">
                    <b-row>
                        <b-col md="6">
                            <h6>Average Monthly Oil Production by Well</h6>
                            <LineChart v-if="avgOilProduct.length != 0" :chartData="avgOilChartData"/>
                        </b-col>
                        <b-col md="6">
                            <h6>Average Monthly Gas Production by Well</h6>
                            <LineChart v-if="avgGasProduct.length != 0" :chartData="avgGasChartData"/>
                        </b-col>
                        <b-col md="6">
                            <h6>Performance Ranker</h6>
                            <LineChart v-if="prodPerformanceRanker.length !== 0" :chartData="performanceRankerChartData"/>
                        </b-col>
                        <b-col md="6">
                            <h6>Performance Ranker</h6>
                            <MultiLineChart v-if="performancePlot.Data.length !== 0" :chartData="performancePlotChart"/>
                        </b-col>
                    </b-row>
                </b-col>
                <b-col md="3" class="ml-auto">
                    <template v-if="prodStatistics">
                        <ul>
                            <li>
                                <span v-if="prodStatistics.PercentageOilGain[0] == '-'">
                                    <b-icon icon="caret-down-fill"/>
                                </span>
                                <span v-else>
                                    <b-icon icon="caret-up-fill"/>
                                </span>
                                <span>{{ prodStatistics.PercentageOilGain }}</span>
                            </li>
                            <li>{{ prodStatistics.CurrentOil }} | {{ prodStatistics.PreviousOil }}</li>
                        </ul>
                        <ul>
                            <li>
                                <span v-if="prodStatistics.PercentageGasGain[0] == '-'">
                                    <b-icon icon="caret-down-fill"/>
                                </span>
                                <span v-else>
                                    <b-icon icon="caret-up-fill"/>
                                </span>
                                <span>{{ prodStatistics.PercentageGasGain }}</span>
                            </li>
                            <li>{{ prodStatistics.CurrentGas }} | {{ prodStatistics.PreviousGas }}</li>
                        </ul>
                        <ul>
                            <li class="font-weight-bolder">Well Count Summary</li>
                            <li>Total No of well: {{ prodStatistics.WellCount }}</li>
                            <li>Total No of prod well: {{ prodStatistics.ProducingWellCount }}</li>
                            <li>Total No of Shut-in well: {{ prodStatistics.ShutInWellCount }}</li>
                        </ul>
                        <ul>
                            <li class="font-weight-bolder">Average Production</li>
                            <li>Average Oil production: {{ prodStatistics.AverageOil }}</li>
                            <li>Average Gas production: {{ prodStatistics.AverageGas }}</li>
                            <li>Average Water production: {{ prodStatistics.AverageWater }}</li>
                        </ul>
                    </template>
                </b-col>
            </b-row>
        </b-container>
    </section>
</template>

<script lang='ts'>
import {Vue, Component} from 'vue-property-decorator';
import {namespace} from 'vuex-class';

import LineChart from '@/components/lineChart/lineChart.vue';
import MultiLineChart from '@/components/multiLineChart/multiLineChart.vue';
import WellsSelector from '@/components/wellsSelector/wellsSelector.vue';

import { AvgResponse, ChartData, PerformancePlot, PerformancePlotData, PerformanceRanker, ProductionStatistics, RequestObject } from '@/types/Utility';

const indexStore = namespace('index');

@Component({
    components: {
        LineChart,
        WellsSelector,
        MultiLineChart
    }
})
export default class Index extends Vue {
    @indexStore.Action fetchProductionStatistics!: (request: RequestObject) => Promise<ProductionStatistics>;
    @indexStore.Action fetchAvgOilProduction!: (request: RequestObject) => Promise<AvgResponse[]>;
    @indexStore.Action fetchAvgGasProduction!: (request: RequestObject) => Promise<AvgResponse[]>;
    @indexStore.Action fetchProdPerformanceRanker!: (request: RequestObject) => Promise<PerformanceRanker[]>;
    @indexStore.Action fetchPerformancePlot!: (request: RequestObject) => Promise<PerformancePlot>;

    private prodStatistics: ProductionStatistics | null = null;
    private avgGasProduct: AvgResponse[] = [];
    private avgOilProduct: AvgResponse[] = [];
    private prodPerformanceRanker: PerformanceRanker[] = [];
    private performancePlot: PerformancePlot = {
        Data: [], GasUnit: '', OilUnit: '', WaterUnit: ''
    };

    get avgOilChartData(): ChartData {
        const chartData: ChartData = {
            labels: this.avgOilProduct.map((entry: AvgResponse) => entry.Name),
            datasets: [
                {
                    data: this.avgOilProduct.map((entry: AvgResponse) => entry.Value),
                    backgroundColor: '#ED7D31'
                }
            ]
        }

        return chartData
    }

    get avgGasChartData(): ChartData {
        const chartData: ChartData = {
            labels: this.avgGasProduct.map((entry: AvgResponse) => entry.Name),
            datasets: [
                {
                    data: this.avgGasProduct.map((entry: AvgResponse) => entry.Value),
                    backgroundColor: '#577ECB'
                }
            ]
        }

        return chartData
    }

    get performanceRankerChartData(): ChartData {
        const chartData: ChartData = {
            labels: this.prodPerformanceRanker.map((entry: PerformanceRanker) => entry.AssetName),
            datasets: [
                {
                    data: this.prodPerformanceRanker.map((entry: PerformanceRanker) => entry.OilValue),
                    label: 'Oil Value',
                    backgroundColor: '#1D2D4B'
                },
                {
                    data: this.prodPerformanceRanker.map((entry: PerformanceRanker) => entry.GasValue),
                    label: 'Gas Value',
                    backgroundColor: '#4DBF84'
                },
                {
                    data: this.prodPerformanceRanker.map((entry: PerformanceRanker) => entry.WaterValue),
                    label: 'Water Value',
                    backgroundColor: '#577ECB'
                }
            ]
        }

        return chartData
    }

    get performancePlotChart(): ChartData {
        const chartData: ChartData = {
            labels: this.performancePlot.Data.map((entry: PerformancePlotData, index: number) => String(index)),
            datasets: [
                {
                    data: this.performancePlot.Data.map((entry: PerformancePlotData) => entry.OilRate),
                    label: 'Oil Value',
                    backgroundColor: '#1D2D4B'
                },
                {
                    data: this.performancePlot.Data.map((entry: PerformancePlotData) => entry.GOR),
                    label: 'Gas Value',
                    backgroundColor: '#4DBF84'
                },
                {
                    data: this.performancePlot.Data.map((entry: PerformancePlotData) => entry.WaterCut),
                    label: 'Water Value',
                    backgroundColor: '#577ECB'
                },
            ]
        }

        return chartData
    }

    getWellsData(wellIds: string[]) {
        const requestObject: RequestObject = {
            Ids: wellIds,
            AssetType: 3
        }

        this.avgOilProduct = [];
        this.avgGasProduct = [];
        this.prodPerformanceRanker = [];

        try {
            this.fetchProductionStatistics(requestObject).then(response => {
                this.prodStatistics = response
            })
            this.fetchAvgOilProduction(requestObject).then(response => {
                this.avgOilProduct = response;
            })
            this.fetchAvgGasProduction(requestObject).then(response => {
                this.avgGasProduct = response;
            })
            this.fetchProdPerformanceRanker(requestObject).then(response => {
                this.prodPerformanceRanker = response;
            })
            this.fetchPerformancePlot(requestObject).then(response => {
                this.performancePlot = response;
            })
        } catch(error) {
            console.log(error)
        }
    }
}
</script>

<style lang="scss" scoped>
ul {
    padding-left: 0;
    list-style: none;
}
</style>
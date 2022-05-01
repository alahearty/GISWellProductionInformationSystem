<template>
    <section>
        <div class="panes">
			<div class="left-pane toggle" ref="leftPane">
                <div class="toggler" @click="toggleLeftPane">
                    <b-icon icon="list"/>
                </div>
                <p class="bg-primary py-2 pl-2 mb-0 lato">Asset Explorer</p>
                <div class="content d-flex">
                    <div class="p-3 my-auto mx-auto" v-if="loadingAssetExplorer">
                        <cc-loader type="circular" :height="60">
                            <template #top>
                                <p class="font-weight-bold text-center mb-1">Loading Assets</p>
                            </template>
                        </cc-loader>
                    </div>

                    <ul class="home-asset-explorer pt-3" v-else>
                        <li v-for="(asset, assetIndex) in explorer" :key="assetIndex">
                            <p 
                                @click.stop="toggleVisibility" 
                                class="my-1 d-flex align-items-center">
                                    <b-form-radio
                                        class="mr-1"
                                        v-model="selectedAsset"
                                        :value="{id: asset.Id, name: asset.DisplayName, type: 'OML'}">
                                    </b-form-radio>
                                    OML {{ asset.DisplayName }}
                            </p>
                            <template v-if="asset.Fields">
                                <li v-for="(field, fieldIndex) in asset.Fields" :key="`field-${fieldIndex}`" class="toggle">
                                    <p 
                                        class="py-1 mb-0 d-flex align-items-center" 
                                        style="padding-left: 10px;" 
                                        @click.stop="toggleVisibility">
                                            <b-form-radio
                                                class="mr-1"
                                                v-model="selectedAsset"
                                                :value="{id: field.Id, name: field.DisplayName, type: 'Field'}">
                                            </b-form-radio>
                                            {{ field.DisplayName }}
                                    </p>
                                    <template v-if="field.Wells">
                                        <li v-for="(well, wellIndex) in field.Wells" :key="`well-${wellIndex}`" class="toggle">
                                            <p 
                                                class="py-1 mb-0 d-flex align-items-center" 
                                                style="padding-left: 35px;" 
                                                @click.stop="viewWell(well)">
                                                    <b-form-radio
                                                        class="mr-1"
                                                        v-model="selectedAsset"
                                                        :value="{id: well.Id, name: well.DisplayName, type: 'Well'}">
                                                    </b-form-radio>
                                                    {{ well.DisplayName }}
                                            </p>
                                        </li>
                                    </template>
                                </li>
                            </template>
                        </li>
                    </ul>
                </div>
            </div>
			<div class="center-pane d-flex flex-column">
                <div style="height: 70%;" class="px-4">
                    <b-container class="h-100">
                        <b-row class="d-flex h-100">
                            <b-col md="12" class="my-auto">
                                <div class="chart-wrapper position-relative">
                                    <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                    </b-overlay>
                                    <p class="mb-0 text-center font-weight-bold">{{ selectedAsset.name }} Performance plot</p>
                                    <ChartMultiLine :chartData="multilineChartData"/>
                                </div>
                            </b-col>
                        </b-row>
                    </b-container>
                </div>
                <div style="height: 30%;" class="bottom-pane px-4">
                    <template v-if="productionStatistics">
                        <div>
                            <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                            </b-overlay>
                            <template v-else>
                                <h6 class="d-flex mb-0">
                                    OIL-BOEPD
                                    <span class="ml-auto mb-3" style="font-size: 20px;" :style="numberStyle(productionStatistics.PercentageOilGain)">
                                        <span>
                                            <b-icon icon="caret-down-fill" font-scale="1.4" v-if="isNegative(productionStatistics.PercentageOilGain)"/>
                                            <b-icon icon="caret-up-fill" font-scale="1.4" v-else/>
                                        </span>
                                        {{ productionStatistics.PercentageOilGain }}
                                    </span>
                                </h6>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Current Month</td>
                                            <td>{{ productionStatistics.CurrentOil }}</td>
                                        </tr>
                                        <tr>
                                            <td>Previouse Month</td>
                                            <td>{{ productionStatistics.PreviousOil }}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </template>
                        </div>
                        <div>
                            <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                            </b-overlay>
                            <template v-else>
                                <h6 class="d-flex mb-0">
                                    GAS-BOEPD
                                    <span class="ml-auto mb-3" style="font-size: 20px;" :style="numberStyle(productionStatistics.PercentageGasGain)">
                                        <span>
                                            <b-icon icon="caret-down-fill" font-scale="1.4" v-if="isNegative(productionStatistics.PercentageGasGain)"/>
                                            <b-icon icon="caret-up-fill" font-scale="1.4" v-else/>
                                        </span>
                                        {{ productionStatistics.PercentageGasGain }}
                                    </span>
                                </h6>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Current Month</td>
                                            <td>{{ productionStatistics.CurrentGas }}</td>
                                        </tr>
                                        <tr>
                                            <td>Previouse Month</td>
                                            <td>{{ productionStatistics.CurrentGas }}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </template>
                        </div>
                        <div>
                            <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                            </b-overlay>
                            <template v-else>
                                <h6>Well Count Summary</h6>
                                <p>Total No. of wells: <span class="font-weight-bold">{{ productionStatistics.WellCount }}</span></p>
                            </template>
                        </div>
                        <div>
                            <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                            </b-overlay>
                            <template v-else>
                                <h6>Average Production</h6>
                                <p>Avg. Oil Prod: <span class="font-weight-bold">{{ productionStatistics.AverageOil }}</span></p>
                                <p>Avg. Gas Prod: <span class="font-weight-bold">{{ productionStatistics.AverageGas }}</span></p>
                                <p>Avg. Water Prod: <span class="font-weight-bold">{{ productionStatistics.AverageWater }}</span></p>
                            </template>
                        </div>
                    </template>
                </div>
			</div>
		</div>
    </section>
</template>

<script lang='ts'>
import {Vue, Component, Watch} from 'vue-property-decorator';
// import Pattern from 'patternomaly';

import CcLoader from '@/components/cc-loader/cc-loader.vue';
import ChartMultiLine from '@/components/chart-multiline/chart-multiline.vue';

import { ChartData, Explorer, PerformancePlotData, ProductionStatistics as ProductionStatisticsType } from '@/types/Utility';

import { fetchPerformanceData, getPerformanceData, getProductionStatistics, fetchProductionStatistics, getExplorer, fetchExplorer  } from '../../store';

type AssetName = 'OML' | 'Field' | 'Reservoir' | 'Well';

type SelectedAsset = { id: string; name: string; type: AssetName };

@Component({
    components: {
        ChartMultiLine,
        CcLoader,
    }
})
export default class ProductionStatistics extends Vue {
    private loading = false;
    private selectedAsset: SelectedAsset = {id: '', name: '', type: 'OML'};
    @Watch('selectedAsset', { deep: true})
    onSelectedAssetChange() {
        this.loading = true;
        this.getAssetData()
    }
    private loadingAssetExplorer = false;

    private showOptions = false;

    get explorer(): Explorer[]  {
        return getExplorer(this.$store);
    }
    
    private toggleVisibility(event: MouseEvent): void {
        const targetEl = event.target as HTMLElement;
        const parent = targetEl.parentElement;

        const parentChildren = parent!.children;
        for (let i = 0; i < parentChildren.length; i++) {
            if (parentChildren[i] !== targetEl) {
                parentChildren[i].classList.toggle('toggle')
            }
        }
    }

    private assetNumber = {
        'OML': 0,
        'Field': 1,
        'Reservoir': 2,
        'Well': 3
    };

    private formatNumber(num: number) {
        return new Intl.NumberFormat('en-GB', {
        }).format(num)
    }

    private numberStyle(digits: string) {
        let sign = '';
        if (digits) {
            sign = digits.slice(0, 1);
        }
        return {
            color: sign === '-' ? 'red' : 'green'
        }
    }

    private isNegative(digits: string): boolean {
        if (digits) {
            if (digits.slice(0, 1) === '-') {
                return true
            }
        }
        return false
    }

    get performanceData(): PerformancePlotData[] {
        return getPerformanceData(this.$store)
    }

    get productionStatistics(): ProductionStatisticsType | undefined {
        return getProductionStatistics(this.$store)
    }

    get multilineChartData(): ChartData | null {
        const data: PerformancePlotData[] = this.performanceData.slice(0, 200)
            .sort((perf, perf2) => perf.Date.localeCompare(perf2.Date))

        return {
            labels: data.map((data) => data.Date).sort(),
            datasets: [
                {
                    data: data.map((data) => data.OilRate),
                    label: 'Oil Rate',
                    // backgroundColor: Pattern.draw('diagonal-right-left', 'rgb(255,192,203, .9)'),
                    borderColor: '#E54D44',
                    borderWidth: 2,
                    yAxisID: 'Oil Rate',
                    spanGroups: true,
                    fill: true,
                    type: 'bar',
                    pointRadius: 0,
                    lineTension: 0,
                },
                {
                    data: data.map((data) => data.WaterCut),
                    label: 'Water Cut',
                    backgroundColor: 'blue',
                    borderColor: 'steelblue',
                    borderWidth: 2,
                    yAxisID: 'Water Cut',
                    spanGroups: true,
                    fill: false,
                    type: 'line',
                    pointRadius: 0,
                    lineTension: 0,
                },
                {
                    data: data.map((data) => data.GOR),
                    label: 'GOR',
                    backgroundColor: 'teal',
                    borderColor: '#2ECC71',
                    borderWidth: 2,
                    yAxisID: 'GOR',
                    spanGroups: true,
                    type: 'line',
                    fill: false,
                    pointRadius: 0,
                    lineTension: 0,
                },
                {
                    data: data.map((data) => data.CumOil),
                    label: 'CUM',
                    backgroundColor: 'red',
                    borderColor: 'red',
                    borderWidth: 2,
                    yAxisID: 'CUM',
                    spanGroups: true,
                    type: 'line',
                    fill: false,
                    pointRadius: 0,
                    lineTension: 0,
                }
            ]
        }
    }

    private assetOptions: string[] = ['OML', 'Field', 'Reservoir', 'Well', 'None'];

    private toggleLeftPane() {
        const leftPane = this.$refs['leftPane'] as HTMLDivElement;
        leftPane.classList.toggle('toggle')
    }

    private getAssetData() {
        const fetchStat = fetchProductionStatistics(this.$store, {
            Id: this.selectedAsset.id,
            AssetType: this.assetNumber[this.selectedAsset.type]
        });
        const fetchPerformance = fetchPerformanceData(this.$store, {
            Id: this.selectedAsset.id,
            AssetType: this.assetNumber[this.selectedAsset.type]
        });

        Promise.all([fetchStat, fetchPerformance])
            .finally(() => {
                this.loading = false
            })
    }

    mounted() {
        if (this.explorer.length === 0) {
            this.loadingAssetExplorer = true;
            fetchExplorer(this.$store)
                .finally(() => {
                    this.loadingAssetExplorer = false;
                })
        }

        window.addEventListener('click', () => {
            this.showOptions = false;
        })
    }
}
</script>

<style lang="scss" scoped>
@import "@/scss/color.scss";

.chart-wrapper {
    height: 400px;
}

.content {
    height: 100%;
}

.loading-rate {
    height: 167px;
    border: 5px dashed #E7E7E7;
    border-radius: 20px;
}

table {
    width: 100%;
}

tr {
    td:first-of-type {
        font-weight: bold;
        margin-bottom: 5px;
    }

    td:last-of-type {
        text-align: right;
    }
}

.panes {
	display: flex;
	position: relative;
	height: 100vh;

	.center-pane {
		margin-left: 20%;
		width: 80%;
		height: 100vh;
		overflow-y: auto;
		display: flex;
	}

	.left-pane, .right-pane {
		position: absolute;
		top: 0;
		bottom: 0;
		background-color: $color-secondary-light;
	}

	.left-pane {
		// overflow: hidden;
		z-index: 20;
        width: 20%;
        transition: 0.5s;
                
        .toggler {
            display: none;
            cursor: pointer;
        }

        .assets {
            overflow-y: auto;
            height: calc(100vh - 130px);
        }
    }

    .bottom-pane {
        background-color: $color-secondary-light;
        padding-top: 10px;
        padding-bottom: 10px;
        border-left: 1px solid $color-secondary;
        display: flex;
        justify-content: space-between;
        overflow-x: auto;

        & > div {
            height: 80%;
            margin: auto 0;
            width: 23%;
            background-color: #fff;
            border-radius: 4px;
            padding: 10px;
            position: relative;

            &:last-of-type {
                margin-bottom: 20px;
            }
            
            h6 {
                font-weight: bold;
                margin-bottom: 20px;
            }

            p {
                margin-bottom: 10px;
            }
        }
    }

	.right-pane {
		right: 0;
    }
    
    
    @media(max-width: 1065px) {
        .left-pane {
            position: fixed;
            width: 30%;
            top: 42px;

            &.toggle {
                transform: translateX(-100%);
            }

            .toggler {
                display: flex;
                align-items: center;
                justify-content: center;
                position: absolute;
                right: -37px;
                height: 37px;
                width: 37px;
            }
        }

        .center-pane {
            width: 100%;
            margin-left: 0;
        }
    }
}

.home-asset-explorer {
    list-style-type: none;
    padding-left: 0;

    li {
        padding-left: 20px;

        &.toggle {
            display: none;
        }
    }

    > li {
        > p:first-of-type {
            border-left: none;
        }
    }

    p {
        cursor: pointer;
        // border-left: 1px dashed gray;
    }
}


.content {
    height: 100%;
    overflow-y: auto;
}
</style>
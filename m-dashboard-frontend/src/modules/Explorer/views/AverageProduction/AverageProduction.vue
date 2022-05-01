<template>
    <section>
        <div class="panes">
			<div class="left-pane toggle" ref="leftPane">
                <div class="toggler" @click="toggleLeftPane">
                    <b-icon icon="list"/>
                </div>
                <div class="bg-primary py-2 px-2 mb-0 lato d-flex align-items-center justify-content-between">
                    <p class="mb-0">Asset Explorer</p>
                    <b-button size="sm" @click="plotChart">Plot Charts</b-button>
                </div>
                <p class="border-bottom pl-3 py-2 mb-0 my-1 d-flex align-items-center">
                    <b-form-checkbox
                        class="mr-1"
                        v-model="selectAll"
                        :value="true"
                        :unchecked-value="false">
                    </b-form-checkbox>
                    Select All
                </p>
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
                                class="my-1 d-flex align-items-center"
                                style="padding-left: 10px;"
                                @click.right.prevent.stop="showNodeOptions($event, 'OML', asset)">
                                    <b-form-checkbox
                                        class="mr-1"
                                        v-model="selectedWells"
                                        @change="assetChange(asset.Id, 'OML')"
                                        :value="asset.Id">
                                        <!-- :value="`OML-${asset.Id}`"> -->
                                    </b-form-checkbox>
                                    {{ asset.DisplayName }}
                            </p>
                            <template v-if="asset.Fields">
                                <li v-for="(field, fieldIndex) in asset.Fields" :key="`field-${fieldIndex}`" class="child toggle">
                                    <p 
                                        class="py-1 mb-0 d-flex align-items-center" 
                                        style="padding-left: 10px;" 
                                        @click.stop="toggleVisibility"
                                        @click.right.prevent.stop="showNodeOptions($event, 'Field', field)">
                                            <b-form-checkbox
                                                class="mr-1"
                                                v-model="selectedWells"
                                                @change="assetChange(field.Id, 'field')"
                                                :value="field.Id">
                                            </b-form-checkbox>
                                            {{ field.DisplayName }}
                                    </p>
                                    <template v-if="field.Reservoirs">
                                        <li v-for="(reservoir, reservoirIndex) in field.Reservoirs" :key="`reservoir-${reservoirIndex}`" class="child toggle">
                                            <p 
                                                class="py-1 mb-0 d-flex align-items-center" 
                                                @click.stop="toggleVisibility"
                                                style="padding-left: 10px;">
                                                    <b-form-checkbox
                                                        class="mr-1"
                                                        v-model="selectedWells"
                                                        @change="assetChange(reservoir.Id, 'reservoir')"
                                                        :value="reservoir.Id">
                                                    </b-form-checkbox>
                                                    {{ reservoir.DisplayName }}
                                            </p>
                                            <template v-if="reservoir.DrainagePoints">
                                                <li v-for="(well, wellIndex) in reservoir.DrainagePoints" :key="`well-${wellIndex}`" class="child">
                                                    <p 
                                                        class="py-1 mb-0 d-flex align-items-center" 
                                                        style="padding-left: 10px;">
                                                            <b-form-checkbox
                                                                class="mr-1"
                                                                v-model="selectedWells"
                                                                @change="assetChange(well.Id, 'well')"
                                                                :value="well.Id">
                                                            </b-form-checkbox>
                                                            {{ well.DisplayName }}
                                                    </p>
                                                </li>
                                            </template>
                                        </li>
                                    </template>
                                </li>
                            </template>
                        </li>
                    </ul>
                </div>
            </div>
			<div class="center-pane px-4 py-2">
				<b-container>
                    <b-row class="cards-wrapper">
                        <b-col md="6" class="mt-2 mb-2">
                            <div class="overview-card">
                                <div>
                                    <img src="@/assets/oil.svg" alt="barrel">
                                    <p>Average Oil Production Rate</p>
                                </div>
                                <span class="asset-unit">(bopd)</span>
                                <template v-if="assetRate">
                                    <span class="perc" :class="[percClass(percentageOilIncrease)]">
                                        <span class="success" :class="{'danger': percentageOilIncrease[0] === '-'}">
                                            <b-icon icon="caret-down-fill" v-if="percentageOilIncrease[0] === '-'"/>
                                            <b-icon icon="caret-up-fill" v-else/>
                                        </span>
                                        {{ percentageOilIncrease }}
                                    </span>
                                    <h1 :title="formatNumber(assetRate.accOilRate)">{{ formatNumber(assetRate.accOilRate) }}</h1>
                                    <h5 class="text-secondary">
                                        {{ formatNumber(assetRate.prevAccOilRate) }} 
                                    </h5>
                                </template>
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                            </div>
                        </b-col>
                        <b-col md="6" class="mt-2 mb-2">
                            <div class="overview-card">
                                <div>
                                    <img src="@/assets/water.svg" alt="water">
                                    <p>Average Water Production Rate</p>
                                </div>
                                <span class="asset-unit">(bpd)</span>
                                <template v-if="assetRate">
                                    <span class="perc" :class="[percClass(percentageWaterIncrease)]">
                                        <span class="success" :class="{'danger': percentageWaterIncrease[0] === '-'}">
                                            <b-icon icon="caret-down-fill" v-if="percentageWaterIncrease[0] === '-'"/>
                                            <b-icon icon="caret-up-fill" v-else/>
                                        </span>
                                        {{ percentageWaterIncrease }}
                                    </span>
                                    <h1 :title="formatNumber(assetRate.accWaterRate)">{{ formatNumber(assetRate.accWaterRate) }}</h1>
                                    <h5 class="text-secondary">
                                        {{ formatNumber(assetRate.prevAccWaterRate) }} 
                                    </h5>
                                </template>
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                            </div>
                        </b-col>
                        <b-col md="6" class="mt-2 mb-2">
                            <div class="overview-card">
                                <div>
                                    <img src="@/assets/gas.svg" alt="pump">
                                    <p>Average Gas Production Rate</p>
                                </div>
                                <span class="asset-unit">(MMscf/d)</span>
                                <template v-if="assetRate">
                                    <span class="perc" :class="[percClass(percentageGasIncrease)]">
                                        <span class="success" :class="{'danger': percentageGasIncrease[0] === '-'}">
                                            <b-icon icon="caret-down-fill" v-if="percentageGasIncrease[0] === '-'"/>
                                            <b-icon icon="caret-up-fill" v-else/>
                                        </span>
                                        {{ percentageGasIncrease }}
                                    </span>
                                    <h1 :title="formatNumber(assetRate.accGasRate)">{{ formatNumber(assetRate.accGasRate) }}</h1>
                                    <h5 class="text-secondary">
                                        {{ formatNumber(assetRate.prevAccOilRate) }} 
                                    </h5>
                                </template>
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                            </div>
                        </b-col>
                        <b-col md="6" class="mt-2 mb-3">
                            <div class="overview-card">
                                <div>
                                    <img src="@/assets/water.svg" alt="pump">
                                    <p>Average Condensate Rate</p>
                                </div>
                                <span class="asset-unit">(bpd)</span>
                                <template v-if="assetRate">
                                    <span class="perc" :class="[percClass(percentageCondensateIncrease)]">
                                        <span class="success" :class="{'danger': percentageCondensateIncrease[0] === '-'}">
                                            <b-icon icon="caret-down-fill" v-if="percentageCondensateIncrease[0] === '-'"/>
                                            <b-icon icon="caret-up-fill" v-else/>
                                        </span>
                                        {{ percentageCondensateIncrease }}
                                    </span>
                                    <h1 :title="formatNumber(assetRate.accCondRate)">{{ formatNumber(assetRate.accCondRate) }}</h1>
                                    <h5 class="text-secondary">
                                        {{ formatNumber(assetRate.prevAccCondRate) }} 
                                    </h5>
                                </template>
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                            </div>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col md="6" class="mt-3 border-right">
                            <p class="font-weight-bold">Average Monthly Oil Production (bopd)</p>
                            <div class="chart-wrapper position-relative">
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                                <horizontal-bar-chart :chartData="monthlyOilChartData"/>
                            </div>
                        </b-col>
                        <b-col md="6" class="mt-3">
                            <p class="font-weight-bold">Total Production Rate Comparison (bpd)</p>
                            <div class="chart-wrapper position-relative">
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                                <horizontal-bar-chart :chartData="assetRankChartData" :showLegend="true"/>
                            </div>
                        </b-col>
                        <b-col md="6" class="border-right">
                            <p class="font-weight-bold">Average Monthly Water Production (bpd)</p>
                            <div class="chart-wrapper position-relative">
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                                <horizontal-bar-chart :chartData="monthlyWaterChartData"/>
                            </div>
                        </b-col>
                        <b-col md="6">
                            <p class="font-weight-bold">Average Monthly Gas Production (MMscf/d)</p>
                            <div class="chart-wrapper position-relative">
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                                <horizontal-bar-chart :chartData="monthlyGasChartData"/>
                            </div>
                        </b-col>
                    </b-row>
                </b-container>
			</div>
		</div>
    </section>
</template>

<script lang='ts'>
import {Vue, Component, Watch} from 'vue-property-decorator';

import CcLoader from '@/components/cc-loader/cc-loader.vue';
import HorizontalBarChart from '@/components/chart-horizonal-bar/chart-horizonal-bar.vue';

import { AverageProduction as AverageProductionType, ChartData, DataSet, Explorer, Field, Well  } from '@/types/Utility';

import { fetchAverageProduction, fetchAverageProductionByDate, fetchExplorer, getAverageProduction, getAverageProductionAssetType, getAverageProductionWells, getExplorer, AssetType, setAverageProductionAssetType, getAverageProductionIncreaseInCondensateRate, getAverageProductionIncreaseInOilRate, getAverageProductionIncreaseInGasRate, getAverageProductionIncreaseInWaterRate  } from '../../store';

// type AssetName = 'OML' | 'Field' | 'Reservoir' | 'Well';

type SelectedAsset = { id: string; type?: AssetType; children?: Well[] | Field[] | Explorer[]};

type AccumulatedAssetRate = {
    accOilRate: number;
    accWaterRate: number;
    accGasRate: number;
    accCondRate: number;
    prevAccOilRate?: number;
    prevAccWaterRate?: number;
    prevAccGasRate?: number;
    prevAccCondRate?: number;
    percOilIncrease?: string;
    percWaterIncrease?: string;
    percGasIncrease?: string;
    percCondIncrease?: string;
}

enum Colors {
    Oil = '#E54D44',
    Water = '#425796',
    Condensate = '#a8b7e6',
    Gas = '#2ECC71'
}

@Component({
    components: {
        HorizontalBarChart,
        CcLoader
    }
})
export default class AverageProduction extends Vue {
    private loading = false;
    
    private selectedAsset: SelectedAsset[] = [];

    private averageProduction: AverageProductionType | null = null;

    get percentageCondensateIncrease(): string {
        return getAverageProductionIncreaseInCondensateRate(this.$store) || '0%'
    }
    get percentageOilIncrease(): string {
        return getAverageProductionIncreaseInOilRate(this.$store) || '0%'
    }
    get percentageGasIncrease(): string {
        return getAverageProductionIncreaseInGasRate(this.$store) || '0%'
    }
    get percentageWaterIncrease(): string {
        return getAverageProductionIncreaseInWaterRate(this.$store) || '0%'
    }
    
    private plotChart() {
        this.loading = true;

        fetchAverageProductionByDate(this.$store, {
            Ids: this.selectedWells,
            AssetType: this.assetNumber[this.selectedAssetType]
        })
        .then(response => {
            this.averageProduction = response
        })
        .finally(() => {
            this.loading = false;
        })
    }
    private loadingAssetExplorer = false;

    private assetNumber: Record<AssetType, number> = {
        'OML': 0,
        'field': 1,
        'reservoir': 2,
        'well': 4
    };

    get omlIds(): string {
        return JSON.stringify(this.explorer.map(asset => asset.Id).sort());
    }

    get sortedSelectedWells(): string {
        return JSON.stringify(this.selectedWells.sort());
    }

    private selectedWells: string[] = [];
    @Watch('selectedWells')
    onSelectedWellsChange() {
        if (this.omlIds == this.sortedSelectedWells) {
            this.selectAll = true;
        }
    }
    
    private selectAll = false;
    @Watch('selectAll')
    onSelectAllChange() {
        if (this.selectAll) {
            this.selectedWells = [];
            this.explorer.forEach(lease => {
                this.selectedWells.push(lease.Id)
            })
        } else {
            this.selectedWells = []
        }
    }
    

    private selectedAssetType: AssetType = 'OML';
    assetChange(assetId: string, assetType: AssetType) {

        if (this.selectedAssetType !== assetType) {
            this.selectedWells = [assetId];
            this.selectedAssetType = assetType;
        }
        setAverageProductionAssetType(this.$store, assetType)
    }

    get explorer(): Explorer[]  {
        return getExplorer(this.$store);
    }

    private percClass(perc: string) {
        const firstChar = perc[0];
        if (firstChar === '-') {
            return 'danger'
        }
        return 'success'
    }
    
    private formatDate(dateString: string): string {
        const months: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        const [year, month] = dateString.split('-');

        return `${months[Number(month) - 1]} ${year}`
    }
    
    private formatNumber(num: number) {
        return new Intl.NumberFormat('en-GB', {
        }).format(num)
    }

    private toggleLeftPane() {
        const leftPane = this.$refs['leftPane'] as HTMLDivElement;
        leftPane.classList.toggle('toggle')
    }

    private assetOptions: string[] = ['OML', 'Field', 'Reservoir', 'Well', 'None'];
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

    get assetRate(): AccumulatedAssetRate | null {
        if (this.averageProduction) {
            const oilRates: Record<string, number> = {};
            const prevOilRates: Record<string, number> = {};
            const percOilIncrease: Record<string, string> = {}

            const waterRates: Record<string, number> = {};
            const prevWaterRates: Record<string, number> = {};
            const percWaterIncrease: Record<string, string> = {}

            const gasRates: Record<string, number> = {};
            const prevGasRates: Record<string, number> = {};
            const percGasIncrease: Record<string, string> = {}
            
            const condRates: Record<string, number> = {};
            const prevCondRates: Record<string, number> = {};
            const percCondIncrease: Record<string, string> = {};

            this.averageProduction.AverageRates.forEach(rate => {
                oilRates[rate.AssetName] = rate.CurrentOilRate;
                prevOilRates[rate.AssetName] = rate.PreviousOilRate;
                if (rate.PercentageIncreaseInOilRate) {
                    percOilIncrease[rate.AssetName] = rate.PercentageIncreaseInOilRate.split(' ')[0];
                } else { 
                    percOilIncrease[rate.AssetName] = ''
                }

                waterRates[rate.AssetName] = rate.CurrentWaterRate;
                prevWaterRates[rate.AssetName] = rate.PreviousWaterRate;
                if (rate.PercentageIncreaseInWaterRate) {
                    percWaterIncrease[rate.AssetName] = rate.PercentageIncreaseInWaterRate.split(' ')[0];
                } else {
                    percWaterIncrease[rate.AssetName] = '';
                }

                gasRates[rate.AssetName] = rate.CurrentGasRate;
                prevGasRates[rate.AssetName] = rate.PreviousGasRate;
                if (rate.PercentageIncreaseInGasRate) {
                    percGasIncrease[rate.AssetName] = rate.PercentageIncreaseInGasRate.split(' ')[0];
                } else {
                    percGasIncrease[rate.AssetName] = '';
                }

                condRates[rate.AssetName] = rate.CurrentCondensateRate;
                prevCondRates[rate.AssetName] = rate.PreviousCondensateRate;
                if (rate.PercentageIncreaseInCondensateRate) {
                    percCondIncrease[rate.AssetName] = rate.PercentageIncreaseInCondensateRate.split(' ')[0] || '';
                } else {
                    percCondIncrease[rate.AssetName] = '';
                }
            })

            const assetNames = this.averageProduction.AverageRates.map(rate => rate.AssetName);

            return {
                accOilRate: assetNames.reduce((acc: number, currValue: string) => {
                    return acc + oilRates[currValue]
                }, 0),
                prevAccOilRate: assetNames.reduce((acc: number, currValue: string) => {
                    return acc + prevOilRates[currValue]
                }, 0),
                percOilIncrease: assetNames.reduce((acc: string, currValue: string) => {
                    return String(Number(acc) + Number(percOilIncrease[currValue]))
                }, ''),
                accWaterRate: assetNames.reduce((acc: number, currValue: string) => {
                    return acc + waterRates[currValue]
                }, 0),
                prevAccWaterRate: assetNames.reduce((acc: number, currValue: string) => {
                    return acc + prevWaterRates[currValue]
                }, 0),
                percWaterIncrease: assetNames.reduce((acc: string, currValue: string) => {
                    return String(Number(acc) + Number(percWaterIncrease[currValue]))
                }, ''),
                accGasRate: assetNames.reduce((acc: number, currValue: string) => {
                    return acc + gasRates[currValue]
                }, 0),
                prevAccGasRate: assetNames.reduce((acc: number, currValue: string) => {
                    return acc + prevGasRates[currValue]
                }, 0),
                percGasIncrease: assetNames.reduce((acc: string, currValue: string) => {
                    return String(Number(acc) + Number(percGasIncrease[currValue]))
                }, ''),
                accCondRate: assetNames.reduce((acc: number, currValue: string) => {
                    return acc + condRates[currValue]
                }, 0),
                prevAccCondRate: assetNames.reduce((acc: number, currValue: string) => {
                    return acc + prevCondRates[currValue]
                }, 0),
                percCondIncrease: assetNames.reduce((acc: string, currValue: string) => {
                    return String(Number(acc) + Number(percCondIncrease[currValue]))
                }, ''),
            }
        }

        return null
    }

    get assetRankChartData(): ChartData {
        let labels: string[] = [];
        if (this.averageProduction) {
            labels = this.averageProduction.AverageRates.map((asset) => asset.AssetName);
        }
        const datasets: DataSet[] = [];
        const assetTypes: ('CurrentOilRate' | 'CurrentGasRateBOE' | 'CurrentWaterRate' | 'CurrentCondensateRate')[] = ['CurrentOilRate', 'CurrentWaterRate', 'CurrentGasRateBOE', 'CurrentCondensateRate'];
        const asset = {
            CurrentOilRate: 'Oil',
            CurrentGasRateBOE: 'Gas',
            CurrentWaterRate: 'Water',
            CurrentCondensateRate: 'Condensate'
        }

        if (this.averageProduction) {
            labels.forEach((label: string, index) => {
                assetTypes.forEach((assetType, index2) => {
                    if (datasets[index2] && this.averageProduction) {
                        datasets[index2].data.push(this.averageProduction.AverageRates[index][assetType])
                    } else {
                        let bgColor = '';
                        if (assetType === 'CurrentOilRate') {
                            bgColor = Colors.Oil;
                        } else if (assetType === 'CurrentWaterRate') {
                            bgColor = Colors.Water;
                        } else if (assetType === 'CurrentGasRateBOE') {
                            bgColor = Colors.Gas;
                        } else {
                            bgColor = Colors.Condensate;
                        }
                        datasets.push({ 
                            backgroundColor: bgColor,
                            label: asset[assetType],
                            datalabels: { color: '#fff' },
                            data: [(this.averageProduction as AverageProductionType).AverageRates[index][assetType]]
                        })
                    }
                })
            })
        }
       
        return {
            labels,
            datasets
        }
    }

    get monthlyOilChartData(): ChartData {
        let labels: string[] = [];
        let data: number[] = [];
        if (this.averageProduction) {
            labels = this.averageProduction.AverageRates.map((asset) => asset.AssetName)
            data = this.averageProduction.AverageRates.map((asset) => asset.CurrentOilRate)
        }
        return {
            labels,
            datasets: [
                {
                    data,
                    backgroundColor: Colors.Oil,
                    datalabels: { color: '#fff' },
                }
            ]
        }
    }

    get monthlyGasChartData(): ChartData {
        let labels: string[] = [];
        let data: number[] = [];
        if (this.averageProduction) {
            labels = this.averageProduction.AverageRates.map((asset) => asset.AssetName)
            data = this.averageProduction.AverageRates.map((asset) => asset.CurrentGasRate)
        }
        return {
            labels,
            datasets: [
                {
                    data,
                    backgroundColor: Colors.Gas,
                    datalabels: { color: '#fff' },
                }
            ]
        }
    }

    get monthlyWaterChartData(): ChartData {
        let labels: string[] = [];
        let data: number[] = [];
        if (this.averageProduction) {
            labels = this.averageProduction.AverageRates.map((asset) => asset.AssetName)
            data = this.averageProduction.AverageRates.map((asset) => asset.CurrentWaterRate)
        }
        return {
            labels,
            datasets: [
                {
                    data,
                    backgroundColor: Colors.Water,
                    datalabels: { color: '#fff' },
                }
            ]
        }
    }

    mounted() {
        const averageProduction = getAverageProduction(this.$store)

        if (averageProduction) {
            
            this.averageProduction = averageProduction;
            this.selectedWells = getAverageProductionWells(this.$store);
            this.selectedAssetType = getAverageProductionAssetType(this.$store)
        } else {
            this.loading = true;
            fetchAverageProduction(this.$store)
                .then((response: AverageProductionType) => {
                    if (response.AverageRates) {
                        this.averageProduction = response;
                    }
                })
                .finally(() => {
                    this.selectAll = true;
                    this.loading = false;
                })
        }

        if (getExplorer(this.$store).length === 0) {
            this.loadingAssetExplorer = true;
            this.loading = true;
            fetchExplorer(this.$store)
                .then((assets: Explorer[]) => {
                    assets.forEach(oml => {
                        this.selectedWells.push(oml.Id);
                    })
                })
                .finally(() => {
                    this.loadingAssetExplorer = false;
                    this.loading = false;
                })
        } else {
            this.selectAll = true
        }
    }
}
</script>

<style lang="scss" scoped>
@import "@/scss/color.scss";

%card {
    border: 0.5px solid hsla(0, 0%, 80%, 0.122);
    border-radius: 4px;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.04), 0px 0px 2px rgba(0, 0, 0, 0.06), 0px 0px 1px rgba(0, 0, 0, 0.04);
}

.charts-card {
    @extend %card;
}

.cards-wrapper {
    overflow-x: auto;
}

.chart-wrapper {
    height: calc(100vh - 330px);
    display: flex;
    flex-direction: column;
    margin-bottom: 40px;
    flex-grow: 1;
    min-height: 0;

     > div {
        position: relative;
        height: 100%;

    }
    canvas {
        height: calc(100%);
    }
}

.overview-card {
    @extend %card;
    min-height: 158px;
    padding: 18px 25px;  
    // width: 28%;
    overflow: hidden;
    position: relative;

    > p:first-of-type {
        color: #7B7B7B;
        margin-bottom: 10px;
    }

    h1 {
        font-size: 36px;
        margin-bottom: 10px;
    }

    div {
        display: flex;
        font-weight: bold;
        color: #000;

        img {
            height: 20px;
            margin-right: 10px;
        }
    }

    @media (max-width: 830px) {
        h1 {
            font-size: 24px;
        }
    }
}

@media (max-width: 500px) {
    .overview-card {
        width: 100%;
    }
}

.loading-rate {
    height: 167px;
    border: 5px dashed #E7E7E7;
    border-radius: 20px;
}

.asset-unit {
    position: absolute;
    bottom: 10px;
    right: 10px;
}

.perc {
    position: absolute;
    right: 10px;
    top: 43%;

    &.success {
        color: $color-success;
    }
    &.danger {
        color: $color-danger;
    }
}

.panes {
	display: flex;
	position: relative;
	height: calc(100vh - 45px);

	.center-pane {
		margin-left: 20%;
		width: 80%;
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

    @media(max-width: 1065px) {
        .left-pane {
            position: fixed;
            width: 50%;
            top: 42px;

            &.toggle {
                transform: translateX(-100%);
            }

            .toggler {
                display: flex;
                align-items: center;
                justify-content: center;
                position: absolute;
                background-color: #fff;
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


.child {
    border-left: 1px dashed #b8b8b8;
}

.home-asset-explorer {
    list-style-type: none;
    padding-left: 0;

    li {
        margin-left: 1rem;

        &.toggle {
            display: none;
        }
    }

    > li {
        margin-left: 0.5rem;

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
    height: calc(100vh - 148px);
    overflow: auto;
}
</style>
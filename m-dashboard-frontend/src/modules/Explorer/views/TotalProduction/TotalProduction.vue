<template>
    <section class="comp-wrapper">
        <div class="panes">
			<div class="left-pane toggle" ref="leftPane">
                <div class="toggler" @click="toggleLeftPane">
                    <b-icon icon="list"/>
                </div>
                <p class="bg-primary py-2 px-2 mb-0 lato d-flex align-items-center">
                    Asset Explorer
                    <b-form-select v-model="selectedDate" :options="dateOptions" size="sm" class="ml-auto w-50"></b-form-select>
                </p>
                <div class="border-bottom pl-3 pr-2 py-2 mb-0 my-1 d-flex align-items-center justify-content-between">
                    <b-form-checkbox
                        class="mr-1"
                        v-model="selectAll"
                        @change="chooseOMLChildren(explorer)">
                        Select All
                    </b-form-checkbox>
                    <b-button class="ml-auto" size="sm" @click="plotCharts" :disabled="!isAnyAssetSelected">Plot Charts</b-button>
                </div>
                <div class="content">
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
                                @click="toggleVisibility" 
                                class="my-1 d-flex align-items-center"
                                style="padding-left: 10px;"
                                @click.right.prevent.stop="showNodeOptions($event, 'OML', asset)">
                                    <span class="folder-arrow rotate mr-1" :class="{'no-visibility': asset.Fields.length == 0}">
                                        <b-icon icon="chevron-down" font-scale="1"></b-icon>
                                    </span>
                                    <b-form-checkbox
                                        class="mr-0"
                                        v-model="selectedOMLs"
                                        @change="chooseFieldChildren(asset.Id, asset.DisplayName, asset.Fields)"
                                        :value="asset.Id">
                                    </b-form-checkbox>
                                    <span>{{ asset.DisplayName }}</span>
                            </p>
                            <template v-if="asset.Fields">
                                <li v-for="(field, fieldIndex) in asset.Fields" :key="`field-${fieldIndex}`" class="child toggle">
                                    <p 
                                        class="py-1 mb-0 d-flex align-items-center" 
                                        style="padding-left: 10px;" 
                                        @click="toggleVisibility"
                                        @click.right.prevent.stop="showNodeOptions($event, 'Field', field)">
                                            <span class="folder-arrow rotate mr-1" :class="{'no-visibility': field.Reservoirs.length == 0}">
                                                <b-icon icon="chevron-down" font-scale="1"></b-icon>
                                            </span>
                                            <b-form-checkbox
                                                class="mr-0"
                                                v-model="selectedFields"
                                                @change="chooseReservoirChildren(field.Id, field.DisplayName, field.Reservoirs)"
                                                :value="field.Id">
                                            </b-form-checkbox>
                                            <span>{{ field.DisplayName }}</span>
                                    </p>
                                    <template v-if="field.Reservoirs">
                                        <li v-for="(reservoir, reservoirIndex) in field.Reservoirs" :key="`reservoir-${reservoirIndex}`" class="child toggle">
                                            <p 
                                                class="py-1 mb-0 d-flex align-items-center" 
                                                @click="toggleVisibility"
                                                style="padding-left: 10px;">
                                                    <span class="folder-arrow rotate mr-1" :class="{'no-visibility': reservoir.DrainagePoints.length == 0}">
                                                        <b-icon icon="chevron-down" font-scale="1"></b-icon>
                                                    </span>
                                                    <b-form-checkbox
                                                        class="mr-0"
                                                        v-model="selectedReservoirs"
                                                        @change="choosePointChildren(reservoir.Id, reservoir.DisplayName, reservoir.DrainagePoints)"
                                                        :value="reservoir.Id">
                                                    </b-form-checkbox>
                                                    <span>{{ reservoir.DisplayName }}</span>
                                            </p>
                                            <template v-if="reservoir.DrainagePoints">
                                                <li v-for="(well, wellIndex) in reservoir.DrainagePoints" :key="`well-${wellIndex}`" class="child toggle">
                                                    <p 
                                                        class="py-1 mb-0 d-flex align-items-center" 
                                                        style="padding-left: 10px;">
                                                            <b-form-checkbox
                                                                class="ml-3 mr-1"
                                                                v-model="selectedPoints"
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
            <div class="center-pane hide-overlay" v-if="!isAnyAssetSelected">
                <div class="w-100 d-flex flex-column align-items-center justify-content-center">
                    <img src="@/assets/sepal_logo.svg" alt="sepal logo" style="height: 220px">
                    <h6 class="font-weight-bold text-white">Please select an asset on the explorer</h6>
                </div>
            </div>
			<div id="center-pane" class="center-pane px-4 pt-1">
				<b-container>
                    <b-row class="cards-wrapper">
                        <b-col md="6" class="mt-2 mb-2">
                            <div class="overview-card">
                                <div>
                                    <img src="@/assets/oil.svg" alt="barrel">
                                    <p>Total Oil Production Rate</p>
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
                                        <small>({{ formatDate(previousDate) }})</small>
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
                                    <p>Total Water Production Rate</p>
                                </div>
                                <span class="asset-unit">(bpd)</span>
                                <template v-if="assetRate">
                                    <span class="perc" :class="[percClass(percentageWaterIncrease)]">
                                        <span class="success" :class="{'danger': percentageWaterIncrease[0] === '-'}">
                                            <b-icon icon="caret-down-fill" v-if="percentageWaterIncrease[0] === '-'"/>
                                            <b-icon icon="caret-up-fill" v-else/>
                                        </span>
                                        {{ percentageWaterIncrease }} %
                                    </span>
                                    <h1 :title="formatNumber(assetRate.accWaterRate)">{{ formatNumber(assetRate.accWaterRate) }}</h1>
                                    <h5 class="text-secondary">
                                        {{ formatNumber(assetRate.prevAccWaterRate) }} 
                                        <small>({{ formatDate(previousDate) }})</small>
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
                                    <p> Total Gas Production Rate</p>
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
                                        {{ formatNumber(assetRate.prevAccGasRate) }} 
                                        <small>({{ formatDate(previousDate) }})</small>
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
                                    <p> Total Condensate Rate</p>
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
                                        <small>({{ formatDate(previousDate) }})</small>
                                    </h5>
                                </template>
                                <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                </b-overlay>
                            </div>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col md="6" class="mt-3">
                            <div class="charts-card d-flex flex-column">
                                <div class="bg-secondary-light d-flex py-2 w-100">
                                    <b-form-radio 
                                        class="ml-3"
                                        v-for="(asset, assetIndex) in assetTypes" 
                                        :key="assetIndex" 
                                        v-model="selectedAsset" 
                                        :value="asset">
                                            <span class="text-capitalize text-dark">{{ asset }}</span>
                                    </b-form-radio>
                                </div>
                                <p class="font-weight-bold w-100 mb-0 text-center">
                                    Total <span class="text-capitalize">{{ selectedAsset }}</span> Production 
                                    (<span>{{ assetUnit[selectedAsset] }}</span>)
                                </p>
                                <div class="chart-wrapper position-relative p-2">
                                    <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                    </b-overlay>
                                    <pie-chart :chartData="pieChartData"/>
                                </div>
                            </div>
                        </b-col>
                        <b-col md="6" class="mt-3">
                            <div class="charts-card d-flex flex-column">
                                <div class="bg-secondary-light d-flex align-items-center py-2 w-100">
                                    <b-form-checkbox 
                                        class="ml-3"
                                        v-for="(asset, assetIndex) in assetTypes" 
                                        :key="assetIndex" 
                                        v-model="selectedLineAssets" 
                                        :value="asset">
                                            <span class="text-capitalize text-dark">{{ asset }}</span>
                                    </b-form-checkbox>
                                </div>
                                <p class="font-weight-bold mb-0 text-center">
                                    Total Production Rate Per 
                                    <span class="text-capitalize">{{ fetchedAssetType }}</span> (bpd)
                                </p>
                                <p class="mb-0 text-center">(Gas Rate Expressed in boepd)</p>
                                <div class="chart-wrapper position-relative p-2">
                                    <b-overlay v-if="loading" :show="loading" variant="light" no-wrap opacity="0.8">
                                    </b-overlay>
                                    <horizontal-bar-chart :chartData="lineChartData" :showLegend="true"/>
                                </div>
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
import PieChart from '@/components/chart-pie/chart-pie.vue';

import TotalAssetsProduction, { Rate } from '@/types/TotalProduction';
import { ChartData, DataSet, Explorer, Field, Reservoir, Well } from '@/types/Utility';

import { 
    getTotalProduction, 
    getProductionDates, 
    fetchProductionData, 
    fetchTotalRateByDate, 
    getExplorer, 
    fetchExplorer, 
    getTotalProductionWells, 
    setTotalProductionAssetType, 
    getTotalProductionAssetType, 
    getPercentageIncreaseInCondensateRate, 
    getPercentageIncreaseInOilRate, 
    getPercentageIncreaseInWaterRate, 
    getPercentageIncreaseInGasRate, 
    setSelectedAssetType, 
    getSelectedAssetType, 
    getOmlIdToNames, 
    getFieldToNames, 
    getReservoirsToNames, 
    setSelectedOMls, 
    setSelectedFields, 
    setSelectedReservoirs, 
    getSelectedOMls, 
    getSelectedFields, 
    getSelectedReservoirs, 
    getAssetChildren, 
    setAssetChildren 
} from '../../store/index';

type AccumulatedAssetRate = {
    accOilRate: number;
    accWaterRate: number;
    accGasRate: number;
    accGasRateBOE: number;
    accCondRate: number;
    prevAccOilRate?: number;
    prevAccWaterRate?: number;
    prevAccGasRate?: number;
    prevAccCondRate?: number;
    percOilIncrease?: number;
    percWaterIncrease?: number;
    percGasIncrease?: number;
    percCondIncrease?: number;
}

enum Colors {
    Oil = '#E54D44',
    Water = '#425796',
    Condensate = '#a8b7e6',
    Gas = '#2ECC71'
}

@Component({
    components: {
        CcLoader,
        HorizontalBarChart,
        PieChart,
    }
})
export default class TotalProduction extends Vue {
    private totalProduction: Rate[] | null = null;

    get percentageCondensateIncrease(): string {
        return getPercentageIncreaseInCondensateRate(this.$store) || '0%'
    }
    get percentageOilIncrease(): string {
        return getPercentageIncreaseInOilRate(this.$store) || '0%'
    }
    get percentageGasIncrease(): string {
        return getPercentageIncreaseInGasRate(this.$store) || '0%'
    }
    get percentageWaterIncrease(): string {
        return getPercentageIncreaseInWaterRate(this.$store) || '0%'
    }

    get totalRates(): Rate[] | undefined {
        return getTotalProduction(this.$store)
    }

    get selectedType(): string {
        return getSelectedAssetType(this.$store)
    }

    get isAnyAssetSelected() {
        const count = this.selectedOMLs.length + 
            this.selectedFields.length + 
            this.selectedReservoirs.length + 
            this.selectedPoints.length
        
        return count !== 0
    }

    private loadingAssetExplorer = false;
    private wellIds: string[] = [];

    private loading = false;
    private selectedAsset = 'oil';
    private selectedLineAssets = ['oil', 'water', 'gas', 'condensate'];

    private assetTypes = ['oil', 'water', 'gas', 'condensate'];

    private assetChildren: Record<string, string[]> = {};
    @Watch('assetChildren', { deep: true })
    onAssetChildrenChange() {
        setAssetChildren(this.$store, this.assetChildren)
    }
    get assetIdToName(): Record<string, string> {
        return {
            ...getOmlIdToNames(this.$store),
            ...getFieldToNames(this.$store),
            ...getReservoirsToNames(this.$store)
        }
    }

    private selectedOMLs: string[] = [];
    @Watch('selectedOMLs')
    onSelectedOMLsChange() {
        setSelectedOMls(this.$store, this.selectedOMLs)
        if (this.selectedOMLs.length === this.explorer.length) {
            this.selectAll = true;
        } else {
            this.selectAll = false;
        }
        
        if (this.selectedType === 'Asset') {
            const rates = this.totalRates?.filter(rate => {
                let assetId = '';
                for (const a of Object.keys(this.assetIdToName)) {
                    if (this.assetIdToName[a] == rate.AssetName) {
                        assetId = a;
                    }
                }
                return this.selectedOMLs.includes(assetId)
            })
            this.arrangeData(rates as Rate[], true)
        }
    }
    private selectedFields: string[] = [];
    @Watch('selectedFields')
    onSelectedFieldsChange() {
        setSelectedFields(this.$store, this.selectedFields)
        if (this.selectedType === 'OML') {
            const rates = this.totalRates?.filter(rate => {
                let assetId = '';
                for (const a of Object.keys(this.assetIdToName)) {
                    if (this.assetIdToName[a] == rate.AssetName) {
                        assetId = a;
                    }
                }
                return this.selectedFields.includes(assetId)
            })
            this.arrangeData(rates as Rate[], false)
        }
    }
    private selectedReservoirs: string[] = [];
    @Watch('selectedReservoirs')
    onSelectedReservoirsChange() {
        setSelectedReservoirs(this.$store, this.selectedReservoirs)
        if (this.selectedType == 'field') {
            const rates = this.totalRates?.filter(rate => {
                let assetId = '';
                for (const a of Object.keys(this.assetIdToName)) {
                    if (this.assetIdToName[a] == rate.AssetName) {
                        assetId = a;
                    }
                }
                return this.selectedReservoirs.includes(assetId)
            })
            this.arrangeData(rates as Rate[], false)
        }
    }
    private selectedPoints: string[] = [];
        
    private selectAll = false;

    chooseOMLChildren(omls: Explorer[]) {
        omls.forEach(oml => {
            const childIndex = this.selectedOMLs.indexOf(oml.Id);

            if (childIndex !== -1) {
                if (!this.selectAll) {
                    this.selectedOMLs.splice(childIndex, 1)
                    if (oml.Id in this.assetChildren) {
                        this.$delete(this.assetChildren, oml.Id)
                    }
                }
            } else {
                if (this.selectAll) {
                    this.selectedOMLs.push(oml.Id);
                    this.$set(this.assetChildren, oml.Id, oml.Fields.map(field => field.DisplayName))
                }
            }

            if (oml.Fields) {
                this.chooseFieldChildren(oml.Id, oml.DisplayName, oml.Fields)
            }
        })
    }
    chooseFieldChildren(parentId: string, parentName: string, fields: Field[]) {
        if (!this.selectedOMLs.includes(parentId)) {
                this.$delete(this.assetChildren, parentId)
        } else {
            this.$set(this.assetChildren, parentId, fields.map(field => field.DisplayName))
        }
        fields.forEach(field => {
            const childIndex = this.selectedFields.indexOf(field.Id)

            if (!this.selectedOMLs.includes(parentId)) {
                if (childIndex !== -1) {
                    this.selectedFields.splice(childIndex, 1)
                }
            } else {
                if (childIndex === -1) {
                    this.selectedFields.push(field.Id);
                    
                }
            }

            if (field.Reservoirs) {
                this.chooseReservoirChildren(field.Id, field.DisplayName, field.Reservoirs)
            }
        })
    }
    chooseReservoirChildren(parentId: string, parentName: string, reservoirs: Reservoir[]) {
        if (!this.selectedFields.includes(parentId)) {
            this.$delete(this.assetChildren, parentId)
        } else {
            this.$set(this.assetChildren, parentId, reservoirs.map(reservoir => reservoir.DisplayName))
        }

        reservoirs.forEach(reservoir => {
            const childIndex = this.selectedReservoirs.indexOf(reservoir.Id)

            if (!this.selectedFields.includes(parentId)) {
                if (childIndex !== -1) {
                    this.selectedReservoirs.splice(childIndex, 1)
                }
            } else {
                if (childIndex === -1) {
                    this.selectedReservoirs.push(reservoir.Id);
                }
            }

            this.choosePointChildren(reservoir.Id, reservoir.DisplayName, reservoir.DrainagePoints)
        })
    }
    choosePointChildren(parentId: string, parentName: string, children: Well[]) {
        children.forEach(child => {
            const childIndex = this.selectedPoints.indexOf(child.Id)

            if (!this.selectedReservoirs.includes(parentId)) {
                this.$delete(this.assetChildren, parentId)
                if (childIndex !== -1) {
                    this.selectedPoints.splice(childIndex, 1)
                }
            } else {
                this.$set(this.assetChildren, parentId, children.map(p => p.DisplayName))
                if (childIndex === -1) {
                    this.selectedPoints.push(child.Id);
                }
            }
        })
    }

    private assetUnit = {
        'oil': 'bpd',
        'water': 'bpd',
        'gas': 'MMscf/d',
        'condensate': 'bpd'
    }

    private assetNumber = {
        'OML': 0,
        'field': 1,
        'reservoir': 2,
        'well': 4
    };
    
    private rateLoading = [
        'Loading oil rate',
        'Loading water rate',
        'Loading gas rate'
    ]

    private firstRun = true;
    private singleDeselect = false;
    private selectedDrainagePoint: string[] = [];

    get omlIds(): string {
        return JSON.stringify(this.explorer.map(asset => asset.Id).sort());
    }

    get sortedSelectedWells(): string {
        return JSON.stringify(this.selectedWells.sort());
    }

    private selectedWells: string[] = [];
    
    private previousDate = '';
    private selectedDate = '';
    private fetchedAssetType = 'OML';
    private selectedAssetType: 'OML' | 'field' | 'reservoir' | 'well' = 'OML';
    assetChange(assetId: string, assetType: 'OML' | 'field' | 'reservoir' | 'well') {

        if (this.selectedAssetType !== assetType) {
            this.selectedWells = [assetId];
            this.selectedAssetType = assetType;
            setTotalProductionAssetType(this.$store, assetType)
        }
    }

    private percClass(perc: string) {
        const firstChar = perc[0];
        if (firstChar === '-') {
            return 'danger'
        }
        return 'success'
    }

    private arrangeData(rates: Rate[], entered = false) {
        const selectedType = getSelectedAssetType(this.$store);

        if (selectedType === 'well' || entered) {
            this.totalProduction = rates;
        }
        if (selectedType === 'OML') {
                const OMLs: Record<string, Rate> = {};

                for (const prod in rates) {
                    this.selectedOMLs.forEach(omlId => {
                        if (this.assetChildren[omlId].includes(rates[prod].AssetName)) {
                            if (omlId in OMLs) {
                                OMLs[omlId].CurrentGasRate += rates[prod].CurrentGasRate
                                OMLs[omlId].CurrentGasRateBOE += rates[prod].CurrentGasRateBOE
                                OMLs[omlId].PreviousGasRate += rates[prod].PreviousGasRate

                                OMLs[omlId].CurrentWaterRate += rates[prod].CurrentWaterRate;
                                OMLs[omlId].PreviousWaterRate += rates[prod].PreviousWaterRate;

                                OMLs[omlId].CurrentCondensateRate += rates[prod].CurrentCondensateRate;
                                OMLs[omlId].PreviousCondensateRate += rates[prod].PreviousCondensateRate;

                                OMLs[omlId].CurrentOilRate += rates[prod].CurrentOilRate;
                                OMLs[omlId].PreviousOilRate += rates[prod].PreviousOilRate;
                            } else {

                                OMLs[omlId] = {...rates[prod]};
                                OMLs[omlId].AssetName = this.assetIdToName[omlId]
                            }
                        }
                    })
                }
                this.totalProduction = Object.values(OMLs)
            }
            if (selectedType === 'field') {
                const OMLs: Record<string, Rate> = {};

                for (const prod in rates) {
                    this.selectedFields.forEach(omlId => {
                        if (this.assetChildren[omlId].includes(rates[prod].AssetName)) {
                            if (omlId in OMLs) {
                                OMLs[omlId].CurrentGasRate += rates[prod].CurrentGasRate
                                OMLs[omlId].CurrentGasRateBOE += rates[prod].CurrentGasRateBOE
                                OMLs[omlId].PreviousGasRate += rates[prod].PreviousGasRate

                                OMLs[omlId].CurrentWaterRate += rates[prod].CurrentWaterRate;
                                OMLs[omlId].PreviousWaterRate += rates[prod].PreviousWaterRate;

                                OMLs[omlId].CurrentCondensateRate += rates[prod].CurrentCondensateRate;
                                OMLs[omlId].PreviousCondensateRate += rates[prod].PreviousCondensateRate;

                                OMLs[omlId].CurrentOilRate += rates[prod].CurrentOilRate;
                                OMLs[omlId].PreviousOilRate += rates[prod].PreviousOilRate;
                            } else {

                                OMLs[omlId] = {...rates[prod]};
                                OMLs[omlId].AssetName = this.assetIdToName[omlId]
                            }
                        }
                    })
                }
                this.totalProduction = Object.values(OMLs)
            } if (selectedType === 'reservoir') {
                const OMLs: Record<string, Rate> = {};

                for (const prod in rates) {
                    this.selectedReservoirs.forEach(omlId => {
                        if (this.assetChildren[omlId].includes(rates[prod].AssetName)) {
                            if (omlId in OMLs) {
                                OMLs[omlId].CurrentGasRate += rates[prod].CurrentGasRate
                                OMLs[omlId].CurrentGasRateBOE += rates[prod].CurrentGasRateBOE
                                OMLs[omlId].PreviousGasRate += rates[prod].PreviousGasRate

                                OMLs[omlId].CurrentWaterRate += rates[prod].CurrentWaterRate;
                                OMLs[omlId].PreviousWaterRate += rates[prod].PreviousWaterRate;

                                OMLs[omlId].CurrentCondensateRate += rates[prod].CurrentCondensateRate;
                                OMLs[omlId].PreviousCondensateRate += rates[prod].PreviousCondensateRate;

                                OMLs[omlId].CurrentOilRate += rates[prod].CurrentOilRate;
                                OMLs[omlId].PreviousOilRate += rates[prod].PreviousOilRate;
                            } else {

                                OMLs[omlId] = {...rates[prod]};
                                OMLs[omlId].AssetName = this.assetIdToName[omlId]
                            }
                        }
                    })
                }
                this.totalProduction = Object.values(OMLs)
            }
    }

    private plotCharts() {
        this.loading = true;
        const request = {
            Date: this.selectedDate,
            Ids: this.selectedOMLs,
            AssetType: 0
        }
        let selectedType = 'OML'

        if (this.selectedPoints.length > 0) {
            selectedType = 'well'
            request.AssetType = this.assetNumber['well'];
            request.Ids = this.selectedPoints;
        }
        if (this.selectedReservoirs.length > 0) {
            selectedType = 'reservoir'

            request.AssetType = this.assetNumber['well'];
            request.Ids = this.selectedPoints;
        }
        if (this.selectedFields.length > 0) {
            selectedType = 'field'

            request.AssetType = this.assetNumber['reservoir'];
            request.Ids = this.selectedReservoirs;
        }
        if (this.selectedOMLs.length > 0) {
            selectedType = 'OML'

            request.AssetType = this.assetNumber['field'];
            request.Ids = this.selectedFields;
        }

        fetchTotalRateByDate(this.$store, request)
        .then(response => {
            setSelectedAssetType(this.$store, selectedType)
            this.arrangeData(response.TotalRates, false)
            
            if (response.TotalRates.length > 0) {

                this.fetchedAssetType = this.selectedAssetType;
                for (const rate of response.TotalRates) {
                    if (rate.PreviousGasDate) {
                        this.previousDate = '';
                        this.previousDate = rate.PreviousGasDate;
                        break;
                    }
                }
            }
            
        })
        .catch(error => {
            console.log('/x/', error)
        })
        .finally(() => {
            this.loading = false;
        })
    }


    get explorer(): Explorer[]  {
        return getExplorer(this.$store);
    }

    private getRandomColor(): string {
        const letters = '0123456789ABCDEF'.split('');
        let color = '#';
        for (let i = 0; i < 6; i++ ) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }

    private formatDate(dateString: string): string {
        const months: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        const [year, month] = dateString.split('-');

        return `${months[Number(month) - 1]} ${year}`
    }

    get dateOptions(): Record<string, string>[] {
        const dates: Set<string> = new Set();
        const dateOptions: Record<string, string>[] = []

        if (this.totalProduction) {
            getProductionDates(this.$store).forEach(date => {
                dates.add(date)
            })

            dates.forEach(date => {
                dateOptions.unshift({ value: date, text: this.formatDate(date) })
            })
        }

        return  dateOptions
    }

    
    private toggleVisibility(event: MouseEvent): void {
        const targetEl = event.currentTarget as HTMLElement;
        const parent = targetEl.parentElement;

        const folderArrowEl = targetEl.getElementsByClassName('folder-arrow')[0]
        if (folderArrowEl) {
            folderArrowEl.classList.toggle('rotate')
        }

        const parentChildren = parent!.children;
        for (let i = 0; i < parentChildren.length; i++) {
            if (parentChildren[i] !== targetEl) {
                parentChildren[i].classList.toggle('toggle')
            }
        }
    }

    private formatNumber(num: number) {
        return new Intl.NumberFormat('en-GB', {
        }).format(num)
    }

    private toggleLeftPane() {
        const leftPane = this.$refs['leftPane'] as HTMLDivElement;
        leftPane.classList.toggle('toggle')
    }

    get assetRate(): AccumulatedAssetRate | null {
        if (this.totalProduction) {
            const oilRates: Record<string, number> = {};
            const prevOilRates: Record<string, number> = {};
            const percOilIncrease: Record<string, number> = {}

            const waterRates: Record<string, number> = {};
            const prevWaterRates: Record<string, number> = {};
            const percWaterIncrease: Record<string, number> = {}

            const gasRates: Record<string, number> = {};
            const gasRatesBOE: Record<string, number> = {};
            const prevGasRates: Record<string, number> = {};
            const percGasIncrease: Record<string, number> = {}

            const condRates: Record<string, number> = {};
            const prevCondRates: Record<string, number> = {};
            const percCondIncrease: Record<string, number> = {};

            this.totalProduction.forEach(rate => {
                oilRates[rate.AssetName] = rate.CurrentOilRate;
                prevOilRates[rate.AssetName] = rate.PreviousOilRate;
                if (rate.PercentageIncreaseInOilRate) {
                    percOilIncrease[rate.AssetName] = Number(rate.PercentageIncreaseInOilRate.split(' ')[0]);
                } else { 
                    percOilIncrease[rate.AssetName] = 0
                }

                waterRates[rate.AssetName] = rate.CurrentWaterRate;
                prevWaterRates[rate.AssetName] = rate.PreviousWaterRate;
                if (rate.PercentageIncreaseInWaterRate) {
                    percWaterIncrease[rate.AssetName] = Number(rate.PercentageIncreaseInWaterRate.split(' ')[0]);
                } else {
                    percWaterIncrease[rate.AssetName] = 0;
                }

                gasRates[rate.AssetName] = rate.CurrentGasRate;
                gasRatesBOE[rate.AssetName] = rate.CurrentGasRateBOE;
                prevGasRates[rate.AssetName] = rate.PreviousGasRate;
                if (rate.PercentageIncreaseInGasRate) {
                    percGasIncrease[rate.AssetName] = Number(rate.PercentageIncreaseInGasRate.split(' ')[0]);
                } else {
                    percGasIncrease[rate.AssetName] = 0;
                }
                
                condRates[rate.AssetName] = rate.CurrentCondensateRate;
                prevCondRates[rate.AssetName] = rate.PreviousCondensateRate;
                if (rate.PercentageIncreaseInCondensateRate) {
                    percCondIncrease[rate.AssetName] = Number(rate.PercentageIncreaseInCondensateRate.split(' ')[0]);
                } else {
                    percCondIncrease[rate.AssetName] = 0;
                }
            })

            const assetName = this.totalProduction.map(rate => rate.AssetName)

            return {
                accOilRate: assetName.reduce((acc: number, currValue: string) => {
                    return acc + oilRates[currValue]
                }, 0),
                prevAccOilRate: assetName.reduce((acc: number, currValue: string) => {
                    return acc + prevOilRates[currValue]
                }, 0),
                percOilIncrease: assetName.reduce((acc: number, currValue: string) => {
                    return acc + percOilIncrease[currValue]
                }, 0),
                accWaterRate: assetName.reduce((acc: number, currValue: string) => {
                    return acc + waterRates[currValue]
                }, 0),
                prevAccWaterRate: assetName.reduce((acc: number, currValue: string) => {
                    return acc + prevWaterRates[currValue]
                }, 0),
                percWaterIncrease: assetName.reduce((acc: number, currValue: string) => {
                    return acc + percWaterIncrease[currValue]
                }, 0),
                accGasRate: assetName.reduce((acc: number, currValue: string) => {
                    return acc + gasRates[currValue]
                }, 0),
                accGasRateBOE: assetName.reduce((acc: number, currValue: string) => {
                    return acc + gasRatesBOE[currValue]
                }, 0),
                prevAccGasRate: assetName.reduce((acc: number, currValue: string) => {
                    return acc + prevGasRates[currValue]
                }, 0),
                percGasIncrease: assetName.reduce((acc: number, currValue: string) => {
                    return acc + percGasIncrease[currValue]
                }, 0),
                accCondRate: assetName.reduce((acc: number, currValue: string) => {
                    return acc + condRates[currValue]
                }, 0),
                prevAccCondRate: assetName.reduce((acc: number, currValue: string) => {
                    return acc + prevCondRates[currValue]
                }, 0),
                percCondIncrease: assetName.reduce((acc: number, currValue: string) => {
                    return acc + percCondIncrease[currValue]
                }, 0),
            }
        }

        return null
    }

    get pieChartData(): ChartData & {type: string} | null {
        const assetMap: Record<string, 'TotalOilProduction' | 'TotalGasProduction' | 'TotalWaterProduction' | 'TotalCondensate'> = {
            oil: 'TotalOilProduction',
            gas: 'TotalGasProduction',
            water: 'TotalWaterProduction',
            condensate: 'TotalCondensate'
        }

        const productionData: Record<'TotalOilProduction' | 'TotalGasProduction' | 'TotalWaterProduction' | 'TotalCondensate', Record<string, number>> = {
            TotalOilProduction: {},
            TotalGasProduction: {},
            TotalWaterProduction: {},
            TotalCondensate: {}
        }
        if (this.totalProduction) {
            this.totalProduction.forEach(asset => {
                productionData.TotalOilProduction[asset.AssetName] = asset.CurrentOilRate;
                productionData.TotalWaterProduction[asset.AssetName] = asset.CurrentWaterRate;
                productionData.TotalGasProduction[asset.AssetName] = asset.CurrentGasRate;
                productionData.TotalCondensate[asset.AssetName] = asset.CurrentCondensateRate;
            })
        }

        let labels: string[] = [];
        if (this.totalProduction) {
            labels = this.totalProduction.map(rate => rate.AssetName)
        }
        let data: number[] = [];
        const selectedProperty = assetMap[this.selectedAsset];

        if (this.totalProduction) {
            // const filterdLabels = Object.keys(productionData[selectedProperty]).filter(name => this.selectedAssetsNames.includes(name));
            // labels = filterdLabels
            data = labels.map((well) => { return productionData[selectedProperty][well] })
        } else {
            return null
        }

        const backgroundColor: string[] = [Colors.Oil, Colors.Gas, Colors.Water, Colors.Condensate];
        for (let i = 3; i < data.length; i++) {
            backgroundColor.push(this.getRandomColor())
        }

        return {
            type: 'doughnut',
            labels,
            datasets: [
                {
                    data,
                    backgroundColor,
                    datalabels: { color: '#fff' },
                }
            ]
        }
    }

    get lineChartData(): ChartData | null {
        const assetMap: Record<string, 'TotalOilRate' | 'TotalGasRate' | 'TotalWaterRate' | 'TotalCondensate'> = {
            oil: 'TotalOilRate',
            gas: 'TotalGasRate',
            water: 'TotalWaterRate',
            condensate: 'TotalCondensate'
        }

        const asset = {
            TotalOilRate: 'Oil',
            TotalGasRate: 'Gas',
            TotalWaterRate: 'Water',
            TotalCondensate: 'Condensate'
        }

        // const labels: string[] = this.selectedAssetsNames;
        let labels: string[] = [];
        if (this.totalProduction) {
            labels = this.totalProduction.map(rate => rate.AssetName);
        }
        const datasets: DataSet[] = [];
        const selectedProperties = this.selectedLineAssets.map(asset => assetMap[asset]);

        const productionData: Record<'TotalOilRate' | 'TotalGasRate' | 'TotalWaterRate' | 'TotalCondensate', Record<string, number>> = {
            TotalOilRate: {},
            TotalGasRate: {},
            TotalWaterRate: {},
            TotalCondensate: {}
        }
        if (this.totalProduction) {
            this.totalProduction.forEach(asset => {
                productionData.TotalOilRate[asset.AssetName] = asset.CurrentOilRate;
                productionData.TotalGasRate[asset.AssetName] = asset.CurrentGasRateBOE;
                productionData.TotalWaterRate[asset.AssetName] = asset.CurrentWaterRate;
                productionData.TotalCondensate[asset.AssetName] = asset.CurrentCondensateRate;
            })
        }

        if (this.totalProduction) {
            labels.forEach((label: string) => {
                selectedProperties.forEach((prop: 'TotalOilRate' | 'TotalGasRate' | 'TotalWaterRate' | 'TotalCondensate', index2: number) => {
                    if (datasets[index2]) {
                        datasets[index2].data.push(productionData[prop][label])
                    } else {
                        let bgColor = '';
                        if (prop === 'TotalOilRate') {
                            bgColor = Colors.Oil;
                        } else if (prop === 'TotalWaterRate') {
                            bgColor = Colors.Water;
                        } else if (prop === 'TotalGasRate') {
                            bgColor = Colors.Gas;
                        } else {
                            bgColor = Colors.Condensate;
                        }
                        datasets.push({ 
                            backgroundColor: bgColor, 
                            label: asset[prop],
                            data: [productionData[prop][label]],
                            datalabels: { color: '#fff' },
                        })
                    }
                })
            })

        } else {
            return null
        }

        return {
            labels,
            datasets
        }
    }
    
    mounted() {
        const totalRates = getTotalProduction(this.$store) as Rate[];
        this.selectedOMLs = getSelectedOMls(this.$store)
        this.selectedFields = getSelectedFields(this.$store)
        this.selectedReservoirs = getSelectedReservoirs(this.$store)
        this.assetChildren = getAssetChildren(this.$store);

        if (totalRates) {
            this.arrangeData(totalRates, true)
            if (this.selectedType === 'Asset') {
                this.selectAll = true;
                this.chooseOMLChildren(this.explorer)
            }

            for (const rate of totalRates) {
                if (rate.PreviousGasDate) {
                    this.previousDate = rate.PreviousGasDate;
                    this.selectedDate = rate.CurrentDate;
                    return
                }
            }

            this.selectedWells = getTotalProductionWells(this.$store)
            this.selectedAssetType = getTotalProductionAssetType(this.$store)
        } else {
            this.loading = true;

            fetchProductionData(this.$store)
                .then((response: TotalAssetsProduction) => {
                    if (response.TotalRates.length > 0) {
                        this.totalProduction = response.TotalRates;
                        const firstRate = response.TotalRates[0];

                        this.previousDate = firstRate.PreviousGasDate;
                        this.selectedDate = firstRate.CurrentDate;
                    }
                })
                .finally(() => { 
                    this.loading = false;
                    // this.selectAll = true;
                });
        }

        if (getExplorer(this.$store).length === 0) {
            this.loadingAssetExplorer = true;
            this.loading = true;
            fetchExplorer(this.$store)
                .then((assets: Explorer[]) => {
                    this.chooseOMLChildren(assets)
                })
                .finally(() => {
                    this.loadingAssetExplorer = false;
                    this.loading = false;
                })
        }
    }
}
</script>

<style lang="scss" scoped>
@import "@/scss/color.scss";

.no-visibility {
    visibility: hidden;
}

.folder-arrow {
    transition: 0.2s;
    &.rotate {
        transform: rotateZ(-90deg);
    }
}

.hide-overlay {
    position: fixed;
    z-index: 44;
    top: 42px;
    background-color: rgba(0, 0, 0, 0.849);
    backdrop-filter: blur(1.5px);
}

%card {
    border: 0.5px solid hsla(0, 0%, 80%, 0.122);
    border-radius: 4px;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.04), 0px 0px 2px rgba(0, 0, 0, 0.06), 0px 0px 1px rgba(0, 0, 0, 0.04);
}

.charts-card {
    @extend %card;
}

.asset-unit {
    position: absolute;
    bottom: 10px;
    right: 10px;
}

.cards-wrapper {
    overflow-x: auto;
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

section.comp-wrapper {
    width: 100%;
    background-color: #FCFCFC;
}

.loading-rate {
    height: 167px;
    border: 5px dashed #E7E7E7;
    border-radius: 20px;
}

.inner-mobile-date {
    display: inline-flex;
}

.mobile-date {
    display: none !important;
}

@media (max-width: 777px) {
    .mobile-date {
        display: flex !important;
    }

    .inner-mobile-date {
        display: none;
    }
}

.overview-card {
    @extend %card;
    padding: 18px 25px;  
    min-height: 158px;
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

.panes {
	display: flex;
	position: relative;
	height: calc(100vh - 43px);

	.center-pane {
		margin-left: 20%;
		width: 80%;
        height: calc(100vh - 43px);
		// height: calc(100vh - -148px);
		overflow-y: auto;
		display: flex;
	}

	.left-pane, .right-pane {
        // background-color: 1px solid green;
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

        .resizer {
            position: absolute;
            z-index: 5000;
            top: 0;
            right: 0;
            bottom: 0;
            width: 10px;
            cursor: ew-resize;
        }
    }

	.right-pane {
		right: 0;
    }
    
    
    @media(max-width: 1065px) {
        .left-pane {
            position: fixed;
            width: 50%;
            top: 42px;
            z-index: 45;

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

.asset-explorer {
    position: absolute;
    top: 0;
    bottom: 0;
    right: 0;
    z-index: 4000;
    width: 300px;
    // height: 100vh;
    display: flex;
    white-space: nowrap;
    flex-direction: column;
    background-color: #fff;
    transform: translateX(0);
    transition: .5s;
    border-left: 2px solid $color-secondary;

    .toggler {
        height: 98px;
        width: 20px;
        left: -20px;
        top: 45%;
        background-color: #fff;
        display: flex;
        align-items: center;
        justify-content: center;
        position: absolute;
        border-top-left-radius: 4px;
        border-bottom-left-radius: 4px;
        cursor: pointer;

        span {
            transition: .5s;
        }
    }

    &.toggle {
        transform: translateX(300px);

        .toggler span {
            transform: rotateZ(180deg);
        }
    }
}

.content {
    height: calc(100vh - 148px);
    overflow: auto;
}
</style>
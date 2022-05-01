<template>
    <section class="map-container">
        <ul class="options" v-if="showOptions" :style="optionsPositionStyle">
            <li v-if="selectedAsset.type === 'Well'" @click="viewSelectedWell">Go to location</li>
        </ul>

        <ul class="total-production pl-0 shadow-lg" v-if="assetRate">
            <li class="bg-secondary-light text-dark font-weight-bold py-2">Total Asset Production Rate as at <br>{{ presentProductionDate }}</li>
            <li class="oil">
                <div>
                    <h6 class="d-flex align-items-center">
                        <span class="asset-circle"></span>
                        Oil
                    </h6>
                    <h2 class="mb-0">{{ formatNumber(assetRate.accOilRate) }} <span>(bopd)</span></h2>
                </div>
            </li>
            <li class="water">
                <div>
                    <h6 class="d-flex align-items-center">
                        <span class="asset-circle"></span>
                        Water
                    </h6>
                    <h2 class="mb-0">{{ formatNumber(assetRate.accWaterRate) }} <span>(bpd)</span></h2>
                </div>
            </li>
            <li class="gas">
                <div>
                    <h6 class="d-flex align-items-center">
                        <span class="asset-circle"></span>
                        Gas
                    </h6>
                    <h2 class="mb-0">{{ formatNumber(assetRate.accGasRate) }} <span>(MMscf/d)</span></h2>
                </div>
            </li>
        </ul>

        <GmapMap
            :center="mapCenter"
            :zoom="12"
            map-type-id="terrain"
            @click="showMapInfoWindow = false"
            style="width: 100%; height: 100%"
        >
            <GmapInfoWindow 
                :position="infoWindowPosition"
                :opened="showMapInfoWindow"
                @closeclick="showMapInfoWindow = false">
                    <div class="prod-stats px-4 pb-4 pt-2 position-relative" v-if="!loadingProdStats">
                        
                        <div class="bg-secondary-light d-flex align-items-center py-2 position-absolute" style="top: 10px; left: 20%;">
                            <b-form-radio 
                                class="mx-2 position-relative"
                                v-for="(asset, assetIndex) in assetTypes" 
                                :key="assetIndex" 
                                v-model="selectedAssetType" 
                                :value="asset">
                                    <span class="text-capitalize text-dark font-primary position-relative" style="top: 2px;">{{ asset }}</span>
                            </b-form-radio>
                        </div>

                        <table style="margin-top: 50px;">
                            <thead>
                                <tr>
                                    <th>Drainage Point</th>

                                    <template v-if="selectedAssetType === 'oil'">
                                        <th>Current Oil Rate</th>
                                        <th>Current Oil Date</th>
                                        <th>Previous Oil Rate</th>
                                        <th>Previous Oil Date</th>
                                        <th>Perc. Oil Increase</th>
                                    </template>

                                    <template v-if="selectedAssetType === 'gas'">
                                        <th>Current Gas Rate</th>
                                        <th>Current Gas Date</th>
                                        <th>Previous Gas Rate</th>
                                        <th>Previous Gas Date</th>
                                        <th>Perc. Gas Increase</th>
                                    </template>

                                    <template v-if="selectedAssetType === 'water'">
                                        <th>Current Water Rate</th>
                                        <th>Current Water Date</th>
                                        <th>Previous Water Rate</th>
                                        <th>Previous Water Date</th>
                                        <th>Perc. Water Increase</th>
                                    </template>

                                    <template v-if="selectedAssetType === 'condensate'">
                                        <th>Current Condensate Rate</th>
                                        <th>Current Condensate Date</th>
                                        <th>Previous Condensate Rate</th>
                                        <th>Previous Condensate Date</th>
                                        <th>Perc. Condensate Increase</th>
                                    </template>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(wellRate, wellRateIndex) in wellRates" :key="wellRateIndex">
                                    <td>{{ wellRate.AssetName }}</td>

                                    <template v-if="selectedAssetType === 'oil'">
                                        <td>{{ wellRate.CurrentOilRate }}</td>
                                        <td>{{ parseDate(wellRate.CurrentOilDate) }}</td>
                                        <td>{{ wellRate.PreviousOilRate }}</td>
                                        <td>{{ parseDate(wellRate.PreviousOilDate) }}</td>
                                        <td>
                                            <p class="text-center mb-3" :style="numberStyle(wellRate.PercentageIncreaseInOilRate)">
                                                <span>
                                                    <b-icon icon="caret-down-fill" font-scale="1" v-if="isNegative(wellRate.PercentageIncreaseInOilRate) == '-'"/>
                                                    <b-icon icon="caret-up-fill" font-scale="1" v-else-if="isNegative(wellRate.PercentageIncreaseInOilRate) == '+'"/>
                                                    <b-icon icon="dash" font-scale="1" v-else/>
                                                </span>
                                                {{ wellRate.PercentageIncreaseInOilRate }}
                                            </p>
                                        </td>
                                    </template>

                                    <template v-if="selectedAssetType === 'gas'">
                                        <td>{{ wellRate.CurrentGasRate }}</td>
                                        <td>{{ parseDate(wellRate.CurrentGasDate) }}</td>
                                        <td>{{ wellRate.PreviousGasRate }}</td>
                                        <td>{{ parseDate(wellRate.PreviousGasDate) }}</td>
                                        <td>
                                            <p class="text-center mb-3" :style="numberStyle(wellRate.PercentageIncreaseInGasRate)">
                                                <span>
                                                    <b-icon icon="caret-down-fill" font-scale="1" v-if="isNegative(wellRate.PercentageIncreaseInGasRate) == '-'"/>
                                                    <b-icon icon="caret-up-fill" font-scale="1" v-else-if="isNegative(wellRate.PercentageIncreaseInGasRate) == '+'"/>
                                                    <b-icon icon="dash" font-scale="1" v-else/>
                                                </span>
                                                {{ wellRate.PercentageIncreaseInGasRate }}
                                            </p>
                                        </td>
                                    </template>

                                    <template v-if="selectedAssetType === 'water'">
                                        <td>{{ wellRate.CurrentWaterRate }}</td>
                                        <td>{{ parseDate(wellRate.CurrentWaterDate) }}</td>
                                        <td>{{ wellRate.PreviousWaterRate }}</td>
                                        <td>{{ parseDate(wellRate.PreviousWaterDate) }}</td>
                                        <td>
                                            <p class="text-center mb-3" :style="numberStyle(wellRate.PercentageIncreaseInWaterRate)">
                                                <span>
                                                    <b-icon icon="caret-down-fill" font-scale="1" v-if="isNegative(wellRate.PercentageIncreaseInWaterRate) == '-'"/>
                                                    <b-icon icon="caret-up-fill" font-scale="1" v-else-if="isNegative(wellRate.PercentageIncreaseInWaterRate) == '+'"/>
                                                    <b-icon icon="dash" font-scale="1" v-else/>
                                                </span>
                                                {{ wellRate.PercentageIncreaseInWaterRate }}
                                            </p>
                                        </td>
                                    </template>

                                    <template v-if="selectedAssetType === 'condensate'">
                                        <td>{{ wellRate.CurrentCondensateRate }}</td>
                                        <td>{{ parseDate(wellRate.CurrentCondensateDate) }}</td>
                                        <td>{{ wellRate.PreviousCondensateRate }}</td>
                                        <td>{{ parseDate(wellRate.PreviousCondensateDate) }}</td>
                                        <td>
                                            <p class="text-center mb-3" :style="numberStyle(wellRate.PercentageIncreaseInCondensateRate)">
                                                <span>
                                                    <b-icon icon="caret-down-fill" font-scale="1" v-if="isNegative(wellRate.PercentageIncreaseInCondensateRate) == '-'"/>
                                                    <b-icon icon="caret-up-fill" font-scale="1" v-else-if="isNegative(wellRate.PercentageIncreaseInCondensateRate) == '+'"/>
                                                    <b-icon icon="dash" font-scale="1" v-else/>
                                                </span>
                                                {{ wellRate.PercentageIncreaseInCondensateRate }}
                                            </p>
                                        </td>
                                    </template>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div style="width: 170px;" class="p-4" v-else>
                        <cc-loader type="circular" :height="40">
                            <template #top>
                                <p class="font-weight-bold text-center mb-1">Loading</p>
                            </template>
                        </cc-loader>
                    </div>
            </GmapInfoWindow>
            <GmapMarker
                v-for="(well, wellKey) in wellsLocations"
                :key="wellKey"
                ref="myMarker"
                :position="{lat: well.Latitude, lng: well.Longitude }"
                :clickable="true"
                @click="showProdStats(well)"
                :draggable="false"
                :label="well.WellName" />
        </GmapMap>

        <div class="asset-explorer" ref="asset-explorer">
            <div class="toggler" @click="toggleAssetExplorer">
                <span>
                    <b-icon icon="caret-right-fill"/>
                </span>
            </div>

            <p class="bg-primary py-2 pl-2 mb-0 lato">Asset Explorer</p>
            <p class="border-bottom pl-3 py-2 mb-0 my-1 d-flex align-items-center">
                <b-form-checkbox
                    class="mr-1"
                    v-model="selectAll"
                    @change="chooseOMLChildren(explorer)">
                    Select All
                </b-form-checkbox>
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
                            @click="toggleVisibility" 
                            class="my-1 d-flex align-items-center"
                            style="padding-left: 10px;">
                                <span class="folder-arrow rotate mr-1" :class="{'no-visibility': asset.Fields.length == 0}">
                                    <b-icon icon="chevron-down" font-scale="1"></b-icon>
                                </span>
                                <b-form-checkbox
                                    class="mr-0"
                                    v-model="selectedWells"
                                    @change="chooseFieldChildren(`OML-${asset.Id}`, asset.DisplayName, asset.Fields)"
                                    :value="`OML-${asset.Id}`">
                                </b-form-checkbox>
                                <span>{{ asset.DisplayName }}</span>
                        </p>
                        <template v-if="asset.Fields">
                            <li v-for="(field, fieldIndex) in asset.Fields" :key="`field-${fieldIndex}`" class="child toggle">
                                <p 
                                    class="py-1 mb-0 d-flex align-items-center" 
                                    style="padding-left: 10px;" 
                                    @click="toggleVisibility">
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
                                            style="padding-left: 10px;" 
                                            @click="toggleVisibility">
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
                                                    style="padding-left: 10px;" 
                                                    @click.stop="viewWell(well)">
                                                        <b-form-checkbox
                                                            class="ml-3 mr-1"
                                                            v-model="wellsLocations"
                                                            :value="well">
                                                        </b-form-checkbox>
                                                        <span @click.stop="viewWell(well)">{{ well.DisplayName }}</span>
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
    </section>
</template>

<script lang='ts'>
import {Vue, Component, Watch} from 'vue-property-decorator';

import CcLoader from '@/components/cc-loader/cc-loader.vue';

import TotalAssetsProduction, { Rate } from '@/types/TotalProduction';

import { Explorer, Well, Field, Reservoir, WellRate } from "@/types/Utility";
import { getTotalProduction, fetchProductionData, getExplorer, fetchExplorer, getProductionDates, fetchWellRate, getMapProductionData } from '../../store/index';

type AccumulatedAssetRate = {
    accOilRate: number;
    accWaterRate: number;
    accGasRate: number;
}

type VisibleLocation = {
    latLng: number;
    name: string;
}

type SelectedAsset = {
    asset?: Well | Field | Explorer;
    type?: AssetType;
}

enum AssetType {
    OML = 'OML',
    Field = 'Field',
    Reservoir = 'Reservoir',
    Well = 'Well'
}

@Component({
    components: {
        CcLoader
    }
})
export default class MapExplorer extends Vue {
    private loading = false;
    private loadingProdStats = false;
    private loadingAssetExplorer = false;

    private selectedAssetType = 'oil';
    private assetTypes = ['oil', 'water', 'gas', 'condensate'];

    private wellsLocations: Well[] = [];
    private selectedWells: string[] = [];
    @Watch('selectedWells')
    onSelectedWellsChange() {
        if (this.selectedWells.length === this.explorer.length) {
            this.selectAll = true;
        } else {
            this.selectAll = false;
        }
    }
    private selectedFields: string[] = [];

    private selectedReservoirs: string[] = [];

    chooseOMLChildren(omls: Explorer[]) {
        omls.forEach(oml => {
            const childIndex = this.selectedWells.indexOf(`OML-${oml.Id}`);

            if (childIndex !== -1) {
                if (!this.selectAll) {
                    this.selectedWells.splice(childIndex, 1)
                }
            } else {
                if (this.selectAll) {
                    this.selectedWells.push(`OML-${oml.Id}`);
                }
            }

            if (oml.Fields) {
                this.chooseFieldChildren(`OML-${oml.Id}`, oml.DisplayName, oml.Fields)
            }
        })
    }
    chooseFieldChildren(parentId: string, parentName: string, fields: Field[]) {
        fields.forEach(field => {
            const childIndex = this.selectedFields.indexOf(field.Id)

            if (!this.selectedWells.includes(parentId)) {
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
            const childIndex = this.wellsLocations.indexOf(child)

            if (!this.selectedReservoirs.includes(parentId)) {
                if (childIndex !== -1) {
                    this.wellsLocations.splice(childIndex, 1)
                }
            } else {
                if (childIndex === -1) {
                    this.wellsLocations.push(child);
                }
            }
        })
    }

    private selectedAsset: SelectedAsset = {};

    private infoOptions = {
        pixelOffset: {
            width: 0,
            height: -35,
        },
    };

    private parseDate(dateString: string | null): string {
        if (dateString) {
            const date = new Date(dateString);
    
            return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`
        }

        return '-'
    }

    get presentProductionDate(): string {
        const months: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        let date = '';
        if (this.totalProduction) {
            date = this.totalProduction[0].CurrentDate;
        }
        const [year, month] = date.split('-');

        return `${months[Number(month) - 1]} ${year}`
    }

    private selectAll = false;

    private showOptions = false;
    private optionsPositionStyle: { [cssProperty: string]: string } = { top: '0', left: '0' };
    private showNodeOptions(event: MouseEvent, assetType: AssetType, asset: Well | Field | Explorer) {
        const mousePosition: { [axis: string]: number } = {
            x: event.clientX,
            y: event.clientY,
        };

        this.optionsPositionStyle = {
            top: `${mousePosition.y}px`,
            left: `${mousePosition.x}px`,
        };
        this.selectedAsset = {
            asset: asset,
            type: assetType
        }
        this.showOptions = true;
    }

    private infoWindowPosition = { lat: 0, lng: 0 };
    private showMapInfoWindow = false;

    private mapCenter = {lat: 5.43, lng:  5.73};
    private wellRates: WellRate[] = [];

    private assetNumber = {
        'OML': 0,
        'Field': 1,
        'Reservoir': 2,
        'Well': 3
    };

    private selectedWellName = ''

    private showProdStats(well: Well) {
        this.loadingProdStats = true;
        this.wellRates = [];
        this.selectedWellName = '';
        this.infoWindowPosition = {
            lat: well.Latitude,
            lng: well.Longitude
        }
        this.showMapInfoWindow = true;

        fetchWellRate(this.$store, well.WellId).then((response: WellRate[]) => {
            this.wellRates = response;
            this.selectedWellName = well.WellName;
        }).finally(() => {
            this.loadingProdStats = false;
        })
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

    private viewWell(well: Well) {
        this.mapCenter = {
            lat: well.Latitude,
            lng: well.Longitude
        }
    }

    private numberStyle(digits: string) {
        if (digits === null) {
            return {color: 'gray'}
        }

        let sign = '';

        if (digits) {
            sign = digits.slice(0, 1);
        }
        if (sign === '-') {
            return {color: 'red'}
        }
        if (isNaN(Number(sign)) === false) {
            return {color: 'green'}
        }

        return {color: 'gray'}
    }

    private isNegative(digits: string): string {
        if (digits === null) {
            return '.'
        }
        const firstChar = digits.slice(0, 1);

        if (firstChar === '-') { return '-'}
        if (isNaN(Number(firstChar)) === false) { return '+'}

        return '.'
    }

    get allFieldReservoirs(): {[fieldId: string]: Reservoir[]} {
        const fields: {[fieldId: string]: Reservoir[]}  = {};

        this.explorer.forEach((asset: Explorer) => {
            asset.Fields.forEach((field: Field) => {
                fields[field.Id] = [...field.Reservoirs as Reservoir[]]
            })
        })
        return fields;
    }

    get explorer(): Explorer[]  {
        return getExplorer(this.$store);
    }

    private formatNumber(num: number) {
        return new Intl.NumberFormat('en-GB', {
        }).format(num)
    }

    private toggleAssetExplorer() {
        const assetExplorerEl = this.$refs['asset-explorer'] as HTMLDivElement;
        assetExplorerEl.classList.toggle('toggle')
    }

    get assetRate(): AccumulatedAssetRate | null {
        if (this.totalProduction) {
            const oilRates: Record<string, number> = {};
            const waterRates: Record<string, number> = {};
            const gasRates: Record<string, number> = {};

            this.totalProduction.forEach(rate => {
                oilRates[rate.AssetName] = rate.CurrentOilRate;
                waterRates[rate.AssetName] = rate.CurrentWaterRate;
                gasRates[rate.AssetName] = rate.CurrentGasRate;
            })

            return {
                accOilRate: this.wellIds.filter((well: string) => {
                    return this.selectedWells.includes(well)
                }).reduce((acc: number, currValue: string) => {
                    return acc + oilRates[currValue]
                }, 0),
                accWaterRate: this.wellIds.filter((well: string) => {
                    return this.selectedWells.includes(well)
                }).reduce((acc: number, currValue: string) => {
                    return acc + waterRates[currValue]
                }, 0),
                accGasRate: this.wellIds.filter((well: string) => {
                    return this.selectedWells.includes(well)
                }).reduce((acc: number, currValue: string) => {
                    return acc + gasRates[currValue]
                }, 0),
            }
        }

        return null
    }

    get wellIds(): string[] {
		if (this.totalProduction) {
            return this.totalProduction.map(rate => rate.AssetName)
		}
		return []
	}

    get totalProduction(): Rate[] | undefined {
        const totalRates = getMapProductionData(this.$store);
		if (totalRates) {
			this.selectedWells = totalRates.map(rate => rate.AssetName)
		}
        return totalRates
    }

    mounted() {
        if (!this.totalProduction) {
            this.loading = true;

            fetchProductionData(this.$store)
                .finally(() => { 
                    this.loading = false;
                    this.$forceUpdate();
                });
        }

        if (this.explorer.length === 0) {
            this.loadingAssetExplorer = true;
            fetchExplorer(this.$store)
                .then((assets: Explorer[]) => {
                    for (const asset of assets) {
                        this.chooseFieldChildren(`OML-${asset.Id}`, asset.DisplayName, asset.Fields)
                    }
                    this.selectAll = true;
                })
                .finally(() => {
                    this.loadingAssetExplorer = false;
                })
        } else {
            for (const asset of this.explorer) {
                this.chooseFieldChildren(`OML-${asset.Id}`, asset.DisplayName, asset.Fields)
            }
            this.selectAll = true;
        }
    }
}
</script>

.<style lang="scss" scoped>
@import "@/scss/color.scss";

$color-oil: #E54D44;
$color-water: #425796;
$color-gas: #2ECC71;

.folder-arrow {
    transition: 0.2s;
    &.rotate {
        transform: rotateZ(-90deg);
    }
}

.no-visibility {
    visibility: hidden;
}

table {
    width: 100%;
    margin-top: 10px;
}

tr {
    th {
        width: 50px;
        white-space: nowrap;
        background-color: $color-secondary-light;
        padding: 10px;
    }

    td {
        white-space: nowrap;
        text-align: left;
        padding: 5px 10px;
        width: 20%;
        border: 1px solid $color-secondary;
    }

    td:first-of-type {
        font-weight: bold;
        margin-bottom: 5px;
        padding-right: 15px;
    }

    td:last-of-type {
        // text-align: right;
        padding-left: 15px;
        font-weight: bold;
    }
}

.map-container {
    height: calc(100vh - 43px);
    position: relative;
    width: 100vw;
    overflow-x: hidden;
    overflow-y: hidden;
}

.options {
    background-color: #fff;
    border-radius: 2px;
    padding-left: 0;
    position: fixed;
    z-index: 5000;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.04), 0px 0px 2px rgba(0, 0, 0, 0.06),
    0px 0px 1px rgba(0, 0, 0, 0.04);

    li {
        padding: 5px 10px;
        list-style-type: none;
        cursor: pointer;

        &:hover {
            background-color: $color-secondary-light;
        }
    }
}

.total-production {
    position: absolute;
    z-index: 4000;
    // top: 16%;
    bottom: 10px;
    width: 260px;
    left: 8px;
    border-radius: 4px;
    background-color: #fff;

    li {
        padding: 20px;
        border-bottom: 1px solid #d3d3d3;

        h2 span {
            color: gray;
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

    .content {
        height: 100%;
        overflow-y: auto;
    }
}

.asset-circle {
    display: inline-block;
    height: 15px;
    width: 15px;
    border-radius: 300px;
    margin-right: 10px;
}

.oil {
    .asset-circle {
        background-color: $color-oil;
    }
    
    h2 {
        color: $color-oil;
    }
}

.water {
    .asset-circle {
        background-color: $color-water;
    }

    h2 {
        color: $color-water;
    }
}

.gas {
    .asset-circle {
        background-color: $color-gas;
    }

    h2 {
        color: $color-gas;
    }
}

.circle {
    height: 30px;
    width: 30px;
    border-radius: 400px;
    margin: 0 auto 10px;
    border-width: 2px;
    border-style: solid;

    &.oil {
        border-color: $color-oil;
        + .asset-rate {
            color: $color-oil;
        }
    }

    &.water {
        border-color: #425796;
        + .asset-rate {
            color: $color-water;
        }
    }

    &.gas {
        border-color: #2ECC71;
        + .asset-rate {
            color: $color-gas;
        }
    }
}

.asset-rate {
    font-weight: bold;
    font-size: 18px;
}

.prod-stats {
    & > div {
        background-color: #f7f7f7;
        border-radius: 6px;
        padding: 10px;
        margin-bottom: 20px;

        &:last-of-type {
            margin-bottom: 0;
        }
        
        h6 {
            font-weight: bold;
            margin-bottom: 20px;
        }

        p {
            margin-bottom: 10px;
            white-space: nowrap;
        }
    }
}
</style>
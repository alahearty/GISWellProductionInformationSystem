import { getStoreAccessors } from "vuex-typescript"
import { ActionContext } from "vuex"
import { ApplicationState } from "@/store/applicationState"


import { ErrorHandler, SERVER } from "@/store/server";
import TotalProduction, { Rate } from "@/types/TotalProduction";
import { Asset, AssetExplorer, AverageProduction, AverageProductionRequest, Explorer, PerformancePlotData, ProductionStatistics, RequestByDate, WellRate } from "@/types/Utility";
type ExplorerContext = ActionContext<ExplorerState, ApplicationState>;

export type AssetType = 'OML' | 'field' | 'reservoir' | 'well';
export interface ExplorerState {
    mapProductionData: Rate[] | undefined;

    totalProduction: Rate[] | undefined;
    totalProductionWells: string[];
    totalProductionAssetType: AssetType;
    percentageIncreaseInCondensateRate: string;
    percentageIncreaseInOilRate: string;
    percentageIncreaseInGasRate: string;
    percentageIncreaseInWaterRate: string;

    dates: string[];
    defaultDate: string;
    assetExplorer: AssetExplorer | undefined;
    performanceData: PerformancePlotData[];
    productionStatistics: ProductionStatistics | undefined;
    selectedAssetType: string;
    assetChildren: Record<string, string[]>;
    selectedOMLs: string[];
    selectedFields: string[];
    selectedReservoirs: string[];
    omlIdToNames: Record<string, string>;
    fieldIdToNames: Record<string, string>;
    reservoirIdToNames: Record<string, string>;

    averageProduction: AverageProduction | undefined;
    averageProductionIncreaseInCondensateRate: string;
    averageProductionIncreaseInOilRate: string;
    averageProductionIncreaseInGasRate: string;
    averageProductionIncreaseInWaterRate: string;
    averageProductionWells: string[];
    averageProductionAssetType: AssetType; 
    explorer: Explorer[];
}

export const explorerState: ExplorerState = {
    mapProductionData: undefined,

    totalProduction: undefined,
    totalProductionWells: [],
    totalProductionAssetType: 'OML',
    percentageIncreaseInCondensateRate: '',
    percentageIncreaseInOilRate: '',
    percentageIncreaseInGasRate: '',
    percentageIncreaseInWaterRate: '',

    dates: [],
    defaultDate: '',
    assetExplorer: undefined,
    performanceData: [],
    productionStatistics: undefined,
    averageProduction: undefined,
    selectedAssetType: 'Asset',
    assetChildren: {},
    selectedOMLs: [],
    selectedFields: [],
    selectedReservoirs: [],
    omlIdToNames: {},
    fieldIdToNames: {},
    reservoirIdToNames: {},

    averageProductionIncreaseInCondensateRate: '',
    averageProductionIncreaseInOilRate: '',
    averageProductionIncreaseInGasRate: '',
    averageProductionIncreaseInWaterRate: '',
    averageProductionWells: [],
    averageProductionAssetType: 'OML',
    explorer: []
}

export const explorerStore = {
    namespaced: true,
    state: explorerState,
    mutations: {
        setMapProductionData(state: ExplorerState, productionData: Rate[]): void {
            state.mapProductionData = productionData;
        },

        setTotalProduction(state: ExplorerState, productionData: Rate[]): void {
            state.totalProduction = productionData;
        },
        setTotalProductionWells(state: ExplorerState, wellIds: string[]): void {
            state.totalProductionWells = wellIds;
        },
        setTotalProductionAssetType(state: ExplorerState, assetType: AssetType): void {
            state.totalProductionAssetType = assetType;
        },
        setPercentageIncreaseInCondensateRate(state: ExplorerState, value: string): void {
            state.percentageIncreaseInCondensateRate = value;
        },
        setPercentageIncreaseInOilRate(state: ExplorerState, value: string): void {
            state.percentageIncreaseInOilRate = value;
        },
        setPercentageIncreaseInGasRate(state: ExplorerState, value: string): void {
            state.percentageIncreaseInGasRate = value;
        },
        setPercentageIncreaseInWaterRate(state: ExplorerState, value: string): void {
            state.percentageIncreaseInWaterRate = value;
        },
        setProductionDates(state: ExplorerState, dates: string[]): void {
            state.dates = dates
        },
        setDefaultProductionDate(state: ExplorerState, date: string): void {
            state.defaultDate = date;
        },
        setAssetChildren(state: ExplorerState, assets: Record<string, string[]>): void {
            state.assetChildren = assets;
        },
        setSelectedAssetType(state: ExplorerState, assetType: string): void {
            state.selectedAssetType = assetType
        },
        setSelectedOMls(state: ExplorerState, ids: string[]): void {
            state.selectedOMLs = ids
        },
        setSelectedFields(state: ExplorerState, ids: string[]): void {
            state.selectedFields = ids
        },
        setSelectedReservoirs(state: ExplorerState, ids: string[]): void {
            state.selectedReservoirs = ids
        },
        setOmlIdToNames(state: ExplorerState, ids: Record<string, string>): void {
            state.omlIdToNames = ids
        },
        setFieldIdToNames(state: ExplorerState, ids: Record<string, string>): void {
            state.fieldIdToNames = ids
        },
        setReservoirIdToNames(state: ExplorerState, ids: Record<string, string>): void {
            state.reservoirIdToNames = ids
        },

        setAssetExplorer(state: ExplorerState, assets: AssetExplorer): void {
            state.assetExplorer = assets
        },
        setPerformanceData(state: ExplorerState, performance: PerformancePlotData[]): void {
            state.performanceData = performance
        },
        setProductionStatistics(state: ExplorerState, statistics: ProductionStatistics): void {
            state.productionStatistics = statistics;
        },

        setAverageProduction(state: ExplorerState, averages: AverageProduction): void {
            state.averageProduction = averages;
        },
        setAverageProductionIncreaseInCondensateRate(state: ExplorerState, value: string): void {
            state.averageProductionIncreaseInCondensateRate = value;
        },
        setAverageProductionIncreaseInOilRate(state: ExplorerState, value: string): void {
            state.averageProductionIncreaseInOilRate = value;
        },
        setAverageProductionIncreaseInGasRate(state: ExplorerState, value: string): void {
            state.averageProductionIncreaseInGasRate = value;
        },
        setAverageProductionIncreaseInWaterRate(state: ExplorerState, value: string): void {
            state.averageProductionIncreaseInWaterRate = value;
        },
        setAverageProductionWells(state: ExplorerState, wellIds: string[]): void {
            state.averageProductionWells = wellIds;
        },
        setAverageProductionAssetType(state: ExplorerState, assetType: AssetType): void {
            state.averageProductionAssetType = assetType;
        },
        setExplorer(state: ExplorerState, assets: Explorer[]): void {
            state.explorer = assets
        },
    },
    getters: {
        getMapProductionData(state: ExplorerState): Rate[] | undefined {
            return state.mapProductionData
        },

        getTotalProduction(state: ExplorerState): Rate[] | undefined {
            return state.totalProduction
        },
        getTotalProductionWells(state: ExplorerState): string[] {
            return state.totalProductionWells
        },
        getTotalProductionAssetType(state: ExplorerState): AssetType {
            return state.totalProductionAssetType
        },
        getPercentageIncreaseInCondensateRate(state: ExplorerState): string {
            return state.percentageIncreaseInCondensateRate;
        },
        getPercentageIncreaseInOilRate(state: ExplorerState): string {
            return state.percentageIncreaseInOilRate;
        },
        getPercentageIncreaseInGasRate(state: ExplorerState): string {
            return state.percentageIncreaseInGasRate
        },
        getPercentageIncreaseInWaterRate(state: ExplorerState): string {
            return state.percentageIncreaseInWaterRate;
        },
        getSelectedAssetType(state: ExplorerState): string {
            return state.selectedAssetType;
        },
        getAssetChildren(state: ExplorerState): Record<string, string[]> {
            return state.assetChildren;
        },
        getSelectedOMls(state: ExplorerState): string[] {
            return state.selectedOMLs;
        },
        getSelectedFields(state: ExplorerState): string[] {
            return state.selectedFields;
        },
        getSelectedReservoirs(state: ExplorerState): string[] {
            return state.selectedReservoirs;
        },
        getOmlIdToNames(state: ExplorerState): Record<string, string> {
            return state.omlIdToNames;
        },
        getFieldToNames(state: ExplorerState): Record<string, string> {
            return state.fieldIdToNames;
        },
        getReservoirsToNames(state: ExplorerState): Record<string, string> {
            return state.reservoirIdToNames;
        },

        getProductionDates(state: ExplorerState): string[] {
            return state.dates
        },
        getDefaultProductionDate(state: ExplorerState): string {
            return state.defaultDate
        },
        getAssetExplorer(state: ExplorerState): AssetExplorer | undefined {
            return state.assetExplorer
        },
        getPerformanceData(state: ExplorerState): PerformancePlotData[] {
            return state.performanceData
        },
        getProductionStatistics(state: ExplorerState): ProductionStatistics | undefined {
            return state.productionStatistics;
        },
        getAverageProduction(state: ExplorerState): AverageProduction | undefined {
            return state.averageProduction;
        },
        getAverageProductionIncreaseInCondensateRate(state: ExplorerState): string {
            return state.averageProductionIncreaseInCondensateRate;
        },
        getAverageProductionIncreaseInOilRate(state: ExplorerState): string {
            return state.averageProductionIncreaseInOilRate;
        },
        getAverageProductionIncreaseInGasRate(state: ExplorerState): string {
            return state.averageProductionIncreaseInGasRate;
        },
        getAverageProductionIncreaseInWaterRate(state: ExplorerState): string {
            return state.averageProductionIncreaseInWaterRate;
        },
        getAverageProductionWells(state: ExplorerState): string[] {
            return state.averageProductionWells;
        },
        getAverageProductionAssetType(state: ExplorerState): AssetType {
            return state.averageProductionAssetType;
        },
        getExplorer(state: ExplorerState): Explorer[] {
            return state.explorer
        },
    },
    actions: {
        async fetchProductionData(context: ExplorerContext): Promise<TotalProduction> {
            const request = new Promise<TotalProduction>((resolve, reject) => {
                const response = SERVER.get('api/ProductionRate/total-rate');
                
                response.then((responseObject) => {
                    const serverResponse = responseObject.data as TotalProduction;
                    context.commit('setMapProductionData', serverResponse.TotalRates);

                    context.commit('setTotalProduction', serverResponse.TotalRates);
                    context.commit('setPercentageIncreaseInCondensateRate', serverResponse.PercentageIncreaseInCondensateRate);
                    context.commit('setPercentageIncreaseInOilRate', serverResponse.PercentageIncreaseInOilRate);
                    context.commit('setPercentageIncreaseInGasRate', serverResponse.PercentageIncreaseInGasRate);
                    context.commit('setPercentageIncreaseInWaterRate', serverResponse.PercentageIncreaseInWaterRate);
                    context.commit('setProductionDates', serverResponse.Dates)

                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },

        async fetchProductionDataByDate(context: ExplorerContext, date: string): Promise<TotalProduction> {
            const request = new Promise<TotalProduction>((resolve, reject) => {
                const response = SERVER.post('api/TotalProduction/total-productions-date', { Date: date });

                response.then((responseObject) => {
                    const serverResponse = responseObject.data as TotalProduction;
                    context.commit('setTotalProduction', serverResponse);
                    

                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },

        async fetchTotalRateByDate(context: ExplorerContext, requestObject: RequestByDate): Promise<TotalProduction> {
            const request = new Promise<TotalProduction>((resolve, reject) => {
                const response = SERVER.post('api/ProductionRate/total-rate-date', requestObject);

                response.then((responseObject) => {
                    const serverResponse = responseObject.data as TotalProduction;
                    context.commit('setTotalProduction', serverResponse.TotalRates);
                    context.commit('setTotalProductionWells', requestObject.Ids);

                    context.commit('setPercentageIncreaseInCondensateRate', serverResponse.PercentageIncreaseInCondensateRate);
                    context.commit('setPercentageIncreaseInOilRate', serverResponse.PercentageIncreaseInOilRate);
                    context.commit('setPercentageIncreaseInGasRate', serverResponse.PercentageIncreaseInGasRate);
                    context.commit('setPercentageIncreaseInWaterRate', serverResponse.PercentageIncreaseInWaterRate);
                    
                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });

            return request;
        },

        async fetchAssetExplorer(context: ExplorerContext): Promise<AssetExplorer> {
            const request = new Promise<AssetExplorer>((resolve, reject) => {
                const response = SERVER.get('api/AssetExplorer');

                response.then((responseObject) => {
                    const serverResponse = responseObject.data as AssetExplorer;
                    context.commit('setAssetExplorer', serverResponse);
                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },

        async fetchExplorer(context: ExplorerContext): Promise<Explorer[]> {
            const request = new Promise<Explorer[]>((resolve, reject) => {
                const response = SERVER.get('api/AssetExplorer/home');

                response.then((responseObject) => {
                    const serverResponse = responseObject.data as Explorer[];
                    const a: Record<string, string> = {};
                    const b: Record<string, string> = {};
                    const c: Record<string, string> = {};

                    serverResponse.forEach(oml => {
                        a[oml.Id] = oml.DisplayName

                        oml.Fields.forEach(field => {
                            b[field.Id] = field.DisplayName
                            
                            if (field.Reservoirs) {
                                field.Reservoirs.forEach(reservoir => {

                                    c[reservoir.Id] = reservoir.DisplayName;
                                })
                            }
                        })
                    })
                    context.commit('setReservoirIdToNames', c)
                    context.commit('setFieldIdToNames', b)
                    context.commit('setOmlIdToNames', a)

                    context.commit('setExplorer', serverResponse);
                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },

        async fetchPerformanceData(context: ExplorerContext, asset: Asset): Promise<PerformancePlotData> {
            const request = new Promise<PerformancePlotData>((resolve, reject) => {
                const response = SERVER.post('api/ProductionPerformance/production-performance', asset);
                response.then((responseObject) => {
                    const serverResponse = responseObject.data as PerformancePlotData;
                    context.commit('setPerformanceData', serverResponse);
                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },

        async fetchProductionStatistics(context: ExplorerContext, asset: Asset): Promise<ProductionStatistics> {
            const request = new Promise<ProductionStatistics>((resolve, reject) => {
                const response = SERVER.post('production-statistics', asset);

                response.then((responseObject) => {
                    const serverResponse = responseObject.data as ProductionStatistics;
                    context.commit('setProductionStatistics', serverResponse);
                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },

        async fetchWellRate(context: ExplorerContext, wellId: string): Promise<WellRate[]> {
            const request = new Promise<WellRate[]>((resolve, reject) => {
                const response = SERVER.post('api/ProductionRate/well-rate', {WellId: wellId});

                response.then((responseObject) => {
                    const serverResponse = responseObject.data as WellRate[];
                    // context.commit('setWellRate', serverResponse);
                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },

        // async fetchAverageProduction(context: ExplorerContext, asset: AverageProductionRequest): Promise<AverageProduction[]> {
        //     const request = new Promise<AverageProduction[]>((resolve, reject) => {
        //         const response = SERVER.post('api/AverageProduction/average-production', asset);

        //         response.then((responseObject) => {
        //             const serverResponse = responseObject.data as AverageProduction[];
        //             context.commit('setAverageProduction', serverResponse);
        //             return resolve(serverResponse);
        //         }).catch(err => {
        //             return reject(new ErrorHandler<string>(err).extractErrorMessages());

        //         })
        //     });
        //     return request;
        // },
        async fetchAverageProduction(context: ExplorerContext): Promise<AverageProduction> {
            const request = new Promise<AverageProduction>((resolve, reject) => {
                const response = SERVER.get('api/ProductionRate/average-rate');

                response.then((responseObject) => {
                    const serverResponse = responseObject.data as AverageProduction;
                    context.commit('setAverageProduction', serverResponse);
                    context.commit('setAverageProductionIncreaseInCondensateRate', serverResponse.PercentageIncreaseInCondensateRate);
                    context.commit('setAverageProductionIncreaseInOilRate', serverResponse.PercentageIncreaseInOilRate);
                    context.commit('setAverageProductionIncreaseInGasRate', serverResponse.PercentageIncreaseInGasRate);
                    context.commit('setAverageProductionIncreaseInWaterRate', serverResponse.PercentageIncreaseInWaterRate);
                    
                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },
        async fetchAverageProductionByDate(context: ExplorerContext, asset: AverageProductionRequest): Promise<AverageProduction> {
            const request = new Promise<AverageProduction>((resolve, reject) => {
                const response = SERVER.post('api/ProductionRate/average-rate-asset', asset);

                response.then((responseObject) => {
                    const serverResponse = responseObject.data as AverageProduction;
                    context.commit('setAverageProduction', serverResponse);
                    context.commit('setAverageProductionWells', asset.Ids);
                    return resolve(serverResponse);
                }).catch(err => {
                    return reject(new ErrorHandler<string>(err).extractErrorMessages());

                })
            });
            return request;
        },
    }
}

const { commit, read, dispatch  } = getStoreAccessors<ExplorerState, ApplicationState>('explorer');

const mutations = explorerStore.mutations;
const actions = explorerStore.actions;
const getters = explorerStore.getters;

export const getMapProductionData = read(getters.getMapProductionData);

export const getTotalProduction = read(getters.getTotalProduction);
export const getTotalProductionWells = read(getters.getTotalProductionWells);
export const getTotalProductionAssetType = read(getters.getTotalProductionAssetType);
export const getPercentageIncreaseInCondensateRate = read(getters.getPercentageIncreaseInCondensateRate);
export const getPercentageIncreaseInOilRate = read(getters.getPercentageIncreaseInOilRate);
export const getPercentageIncreaseInGasRate = read(getters.getPercentageIncreaseInGasRate);
export const getPercentageIncreaseInWaterRate = read(getters.getPercentageIncreaseInWaterRate);
export const getSelectedAssetType = read(getters.getSelectedAssetType);
export const getAssetChildren = read(getters.getAssetChildren);
export const getSelectedOMls = read(getters.getSelectedOMls);
export const getSelectedFields = read(getters.getSelectedFields);
export const getSelectedReservoirs = read(getters.getSelectedReservoirs);
export const getOmlIdToNames = read(getters.getOmlIdToNames);
export const getFieldToNames = read(getters.getFieldToNames);
export const getReservoirsToNames = read(getters.getReservoirsToNames);


export const getProductionDates = read(getters.getProductionDates);
export const getDefaultProductionDate = read(getters.getDefaultProductionDate);
export const getAssetExplorer = read(getters.getAssetExplorer);
export const getPerformanceData = read(getters.getPerformanceData);
export const getProductionStatistics = read(getters.getProductionStatistics);

export const getAverageProduction = read(getters.getAverageProduction);
export const getAverageProductionIncreaseInCondensateRate = read(getters.getAverageProductionIncreaseInCondensateRate);
export const getAverageProductionIncreaseInOilRate = read(getters.getAverageProductionIncreaseInOilRate);
export const getAverageProductionIncreaseInGasRate = read(getters.getAverageProductionIncreaseInGasRate);
export const getAverageProductionIncreaseInWaterRate = read(getters.getAverageProductionIncreaseInWaterRate);
export const getAverageProductionWells = read(getters.getAverageProductionWells);
export const getAverageProductionAssetType = read(getters.getAverageProductionAssetType);
export const getExplorer = read(getters.getExplorer);

export const setTotalProduction = commit(mutations.setTotalProduction);
export const setTotalProductionAssetType = commit(mutations.setTotalProductionAssetType);
export const setAverageProductionAssetType = commit(mutations.setAverageProductionAssetType);
export const setProductionDates = commit(mutations.setProductionDates);
export const setDefaultProductionDates = commit(mutations.setDefaultProductionDate);
export const setAssetExplorer = commit(mutations.setAssetExplorer);
export const setAssetChildren = commit(mutations.setAssetChildren);
export const setSelectedAssetType = commit(mutations.setSelectedAssetType);
export const setSelectedOMls = commit(mutations.setSelectedOMls);
export const setSelectedFields = commit(mutations.setSelectedFields);
export const setSelectedReservoirs = commit(mutations.setSelectedReservoirs);

export const fetchProductionData = dispatch(actions.fetchProductionData);
export const fetchProductionDataByDate = dispatch(actions.fetchProductionDataByDate);
export const fetchAssetExplorer = dispatch(actions.fetchAssetExplorer);
export const fetchPerformanceData = dispatch(actions.fetchPerformanceData);
export const fetchTotalRateByDate = dispatch(actions.fetchTotalRateByDate);
export const fetchProductionStatistics = dispatch(actions.fetchProductionStatistics);
export const fetchWellRate = dispatch(actions.fetchWellRate);
export const fetchAverageProduction = dispatch(actions.fetchAverageProduction);
export const fetchAverageProductionByDate = dispatch(actions.fetchAverageProductionByDate);
export const fetchExplorer = dispatch(actions.fetchExplorer);

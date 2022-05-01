import { ActionContext } from "vuex"
import { ApplicationState } from "@/store/applicationState"

import { SERVER } from "@/store/server";
import { AvgResponse, PerformancePlot, PerformanceRanker, ProductionStatistics, RequestObject } from "@/types/Utility";
type IndexContext = ActionContext<IndexState, ApplicationState>;

export type Well = {
    Name: string;
    Id: string;
}

export interface IndexState {
    wells?: Well[];
}

export const indexState: IndexState = {
    wells: []
}

export const indexStore = {
    namespaced: true,
    state: indexState,
    getters: {
        getWells(state: IndexState): Well[] | undefined {
            return state.wells;
        }
    },

    mutations: {
        setWell(state: IndexState, wells: Well[]): void {
            state.wells = wells;
        },

        addWell(state: IndexState, well: Well): void {
            state.wells?.push(well)
        }
    },

    actions: {
        async fetchWells(context: IndexContext): Promise<Well> {
            const request = new Promise<Well>((resolve) => {
                const response = SERVER.get('api/Wells');
                
                response.then((responseObject) => {
                    const serverResponse = responseObject.data as Well;
                    context.commit('setWell', serverResponse);
                    return resolve(serverResponse);
                })
            });
            return request;
        },

        async fetchAvgOilProduction(context: IndexContext, requestObject: RequestObject): Promise<AvgResponse[]> {
            const request = new Promise<AvgResponse[]>((resolve) => {
                const response = SERVER.post('/average-oil-production', requestObject);
                
                response.then((responseObject) => {
                    const serverResponse = responseObject.data as AvgResponse[];
                    return resolve(serverResponse);
                })
            });
            return request;
        },

        async fetchAvgGasProduction(context: IndexContext, requestObject: RequestObject): Promise<AvgResponse[]> {
            const request = new Promise<AvgResponse[]>((resolve) => {
                const response = SERVER.post('/average-gas-production', requestObject);
                
                response.then((responseObject) => {
                    const serverResponse = responseObject.data as AvgResponse[];
                    return resolve(serverResponse);
                })
            });
            return request;
        },

        async fetchProductionStatistics(context: IndexContext, requestObject: RequestObject): Promise<ProductionStatistics> {
            const request = new Promise<ProductionStatistics>((resolve) => {
                const response = SERVER.post('/production-statistics', requestObject);
                
                response.then((responseObject) => {
                    const serverResponse = responseObject.data as ProductionStatistics;
                    return resolve(serverResponse);
                })
            });
            return request;
        },

        async fetchProdPerformanceRanker(context: IndexContext, requestObject: RequestObject): Promise<PerformanceRanker[]> {
            const request = new Promise<PerformanceRanker[]>((resolve) => {
                const response = SERVER.post('/production-performance-ranker', requestObject);
                
                response.then((responseObject) => {
                    const serverResponse = responseObject.data as PerformanceRanker[];
                    return resolve(serverResponse);
                })
            });
            return request;
        },

        async fetchPerformancePlot(context: IndexContext, requestObject: RequestObject): Promise<PerformancePlot> {
            const request = new Promise<PerformancePlot>((resolve) => {
                const response = SERVER.post('/performance-plot', requestObject);
                
                response.then((responseObject) => {
                    const serverResponse = responseObject.data as PerformancePlot;
                    return resolve(serverResponse);
                })
            });
            return request;
        }
    }
}
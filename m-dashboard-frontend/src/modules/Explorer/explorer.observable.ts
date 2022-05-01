import Vue from 'vue';

type State = {
    selectedWells: string[];
}

const wellState = Vue.observable<State>({
    selectedWells: []
})

export const updateSelectedWells = (wells: string[]) => { 
    wellState.selectedWells = wells;
}

export default wellState 
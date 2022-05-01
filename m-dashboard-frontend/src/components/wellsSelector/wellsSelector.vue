<template>
    <section class="shadow-sm">
        <b-form-checkbox-group
            v-model="selectedWells"
            :options="wellOptions"
            name="wells">
                <b-form-checkbox 
                    v-for="(well, wellIndex) in wellOptions" 
                    :key="wellIndex" 
                    :value="well.value">
                        {{ well.text }}
                </b-form-checkbox>
        </b-form-checkbox-group>
    </section>
</template>

<script lang='ts'>
import {Vue, Component, Watch} from 'vue-property-decorator';
import {namespace} from 'vuex-class';

import { Well } from '@/modules/Index/store';
import { FormSelect } from '@/types/Utility';

const indexStore = namespace('index');

@Component
export default class WellsSelector extends Vue {
    @indexStore.State wells!: Well[];
    @indexStore.Action fetchWells!: () => Promise<Well>;

    private selectedWells: string[] = [];
    @Watch('selectedWells')
    onSelectedWellsChange(value: string[]) {
        this.$emit('wellsChange', value)
    }

    get wellOptions(): FormSelect[] {
        return this.wells.map(well => {
            return { value: well.Id, text: well.Name }
        })
    }

    created() {
        this.fetchWells()
    }
}
</script>

<style lang="scss" scoped>
section {
    position: fixed;
    top: 20px;
    bottom: 20px;
    overflow-y: auto;
    border-radius: 4px;
    padding: 10px;
}
</style>
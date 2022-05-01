<template>
    <section>
        <slot name="top">
        </slot>
        <div class="loader" :class="type" :style="loaderStyle" v-if="type !== 'circular'">
            <div class="box" v-if="type === 'single-linear'" :style="boxStyle"></div>
            <div class="box" v-for="int in 5" :key="int" :style="boxStyle" v-else></div>
        </div>
        <div class="circular-loader mx-auto" :style="circularLoaderStyle" v-else>
        </div>
        <slot name="bottom">
        </slot>
    </section>
</template>

<script lang='ts'>
import {Vue, Component, Prop} from 'vue-property-decorator';

enum CcLoaderTypes {
    singleLoader = 'single-loader',
    arrayBox = 'array-box',
    arrayBoxReverse = 'array-box-reverse',
    linear = 'linear',
    circular = 'circular'
}

@Component
export default class CcLoader extends Vue {
    @Prop({ default: CcLoaderTypes.singleLoader }) type!: CcLoaderTypes;
    @Prop({ default: true }) rounded!: string;
    @Prop({ default: 5}) borderWidth!: number;
    @Prop({ default: 10 }) height!: number;

    get loaderStyle(): {[x: string]: string} {
        return {
            borderRadius: this.rounded ? '40px' : '0',
            height: `${this.height}px`
        }
    }

    get circularLoaderStyle(): {[cssProperty: string]: string} {
        return {
            height: `${this.height}px`,
            width: `${this.height}px`,
            borderWidth: `${this.borderWidth}px`
        }
    }

    get boxStyle(): {[x: string]: string} {
        return {
            borderRadius: this.rounded ? '40px' : '0',
        }
    }
}
</script>

<style lang="scss" scoped>
@import '@/scss/color.scss';

.circular-loader {
    border-radius: 20000000px;
    border: 3px solid $color-secondary-light;
    border-right-color: $color-primary;
    animation: rotate 1s linear infinite both;
}

@keyframes rotate {
    from {
        transform: rotateZ(0);
    }
    to {
        transform: rotateZ(360deg);
    }
}

.loader {
    width: 100%;
    // background-color: #eee;
    position: relative;
    overflow: hidden;

    & .box {
        left: 0%;
        width: 10%;
        height: 100%;
        position: absolute;
        background-color: $color-primary;
        animation: single 2.2s ease-in-out 1s alternate infinite both;
    }

    @for $i from 1 to 6 {
        &.array-box .box:nth-of-type(#{$i}) {
            left: -10%;
            width: 10%;
            height: 100%;
            position: absolute;
            background-color: $color-primary;
            animation: array-box 2.2s ease-in-out #{0.2 * $i}s infinite both;
        }
    }

    @for $i from 1 to 6 {
        &.array-box-reverse .box:nth-of-type(#{$i}) {
            left: -15%;
            width: 10%;
            height: 100%;
            position: absolute;
            background-color: $color-primary;
            animation: array-box 2.2s ease-in-out #{0.2 * $i}s alternate infinite;
        }
    }

    @for $i from 1 to 6 {
        &.array-train .box:nth-of-type(#{$i}) {
            left: -10%;
            width: 10%;
            height: 100%;
            position: absolute;
            background-color: $color-primary;
            animation: array-box 2s linear #{0.5 * $i}s infinite;
        }
    }

    @for $i from 1 to 6 {
        &.linear .box:nth-of-type(#{$i}) {
            left: -15%;
            width: 10%;
            height: 100%;
            position: absolute;
            background-color: $color-primary;
            animation: array-box 2.2s ease-in-out #{0.1 * $i}s alternate infinite;
        }
    }
}

@keyframes array-box {
    from {
        transform: translateX(0);
    } 
    to {
        transform: translateX(120% * 10);
    }
}

@keyframes single {
    from {
        transform: translateX(0);
    } 
    to {
        transform: translateX(100% * 9);
    }
}
</style>
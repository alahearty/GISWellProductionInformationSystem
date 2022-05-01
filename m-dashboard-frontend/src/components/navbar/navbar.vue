<template>
    <section class="top-nav d-flex justify-content-between">
        <ul>
            <li>SEPAL Online</li>
        </ul>
        <ul>
            <router-link 
                v-for="(link, linkIndex) in links" 
                :key="linkIndex"
                :to="link.path"
                active-class="active-link"
                tag="li">
                    {{ link.text }}
            </router-link>
            <li class="font-weight-bold logout" @click="logout">
                Logout
            </li>
        </ul>
    </section>
</template>

<script lang='ts'>
import { logout } from '@/views/Auth/store';
import {Vue, Component} from 'vue-property-decorator';

@Component
export default class Navbar extends Vue {
    private links = [
        {
            path: 'map-explorer',
            text: 'Map Explorer'
        },
        {
            path: 'total-production',
            text: 'Asset Production'
        },
        // {
        //     path: 'production-performance',
        //     text: 'Production Performance'
        // },
        // {
        //     path: 'average-production',
        //     text: 'Average Production'
        // }
    ]

    private logout() {
        logout(this.$store)
            .then(() => {
                this.$router.push({ path: '/login' })
            })
    }
}
</script>

<style lang="scss" scoped>
@import "@/scss/color.scss";

.top-nav {
    background-color: #424242;
}

.logout {
    background-color: $color-danger;
    color: #fff;
}

ul {
    list-style: none;
    padding-left: 0;
    display: flex;
    margin-bottom: 0;
    color: #fff;

    li {
        padding: 10px 10px 6px;
        cursor: pointer;
        border-bottom: 6px solid transparent;

        &.active-link {
            border-bottom: 6px solid $color-primary;
            color: $color-primary;
        }
    }
}
</style>
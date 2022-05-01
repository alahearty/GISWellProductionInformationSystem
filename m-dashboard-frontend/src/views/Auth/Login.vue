<template>
    <section class="vh-100 d-flex">
        <b-container class="h-100">
            <b-row class="h-100">
                <b-col cols="11" md="6" lg="4" class="mx-auto my-auto">
                    <h4 class="text-center font-weight-bold">SEPAL Online</h4>
                    <div class="rounded shadow-sm p-4">
                        <b-form-input 
                            placeholder="Username" 
                            v-model="username" 
                            @keydown.enter="login">
                        </b-form-input>
                        <b-form-input 
                            placeholder="Password" 
                            type="password" 
                            v-model="password" 
                            class="mb-4 mt-2" 
                            @keydown.enter="login">
                        </b-form-input>

                        <b-overlay :show="isLoading" spinner-small>
                            <b-button class="bg-primary w-100" @click="login">Login</b-button>
                        </b-overlay>
                    </div>
                </b-col>
            </b-row>
        </b-container>
    </section>
</template>

<script lang='ts'>
import {Vue, Component} from 'vue-property-decorator';
import { login } from './store';

@Component
export default class Login extends Vue {
    private username = '';
    private password = '';

    private isLoading = false;

    private login() {
        this.isLoading = true;

        login(this.$store, {username: this.username, password: this.password})
            .then((response) => {
                this.$bvToast.toast(response, {
                    title: 'Login',
                    solid: true,
                    variant: 'success',
                })
                this.$router.push({ path: '/map-explorer' })
            })
            .catch((error) => {
                this.$bvToast.toast(error, {
                    title: 'Login',
                    solid: true,
                    variant: 'danger',
                })
            })
            .finally(() => {
                this.isLoading = false;
            })
    }
}
</script>
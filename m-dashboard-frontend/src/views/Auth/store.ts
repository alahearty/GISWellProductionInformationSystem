import { ActionContext } from "vuex";
import { ApplicationState } from "@/store/applicationState"
import { User } from "@/types/User";
import { users } from './dummyUsers';
import { getStoreAccessors } from "vuex-typescript";

type AuthContext = ActionContext<AuthState, ApplicationState>;

const saveUserToLocalStorage = (user: User) => {
    localStorage.setItem('orbit', JSON.stringify(user))
}

const clearLocalStorage = () => {
    localStorage.removeItem('orbit')
}

export interface AuthState {
    isLoggedIn: boolean;
}

const state: AuthState = {
    isLoggedIn: false,
}

const mutations = {
    setLogInState(state: AuthState, loginState: boolean) {
        state.isLoggedIn = loginState
    }
}

const getters = {
    getLoggedInState(state: AuthState): boolean {
        return state.isLoggedIn;
    }
}

const actions = {
    login(context: AuthContext, user: User): Promise<string>  {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const itExists = users.findIndex(u => {
                    return u.username === user.username && u.password === user.password
                }) !== -1

                if (itExists) {
                    context.commit('setLogInState', true)
                    saveUserToLocalStorage(user)
                    resolve('Login successful')
                } else {
                    reject('Invalid username or password')
                }
            }, 2000);
        })
    },
    logout(context: AuthContext): Promise<boolean>  {
        return new Promise((resolve, reject) => {
            context.commit('setLogInState', false)
            clearLocalStorage()
            resolve(true)
        })
    },
}

export const authStore = {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
}

const { read, commit, dispatch  } = getStoreAccessors<AuthState, ApplicationState>('auth');

const authMutations = authStore.mutations;
const authActions = authStore.actions;
const authGetters = authStore.getters;

export const getLoggedInState = read(authGetters.getLoggedInState);
export const setLogInState = commit(authMutations.setLogInState);

export const login = dispatch(authActions.login);
export const logout = dispatch(authActions.logout);
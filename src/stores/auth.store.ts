import { defineStore } from 'pinia';

import { router } from '../router';
import { fetchWrapper } from '../services';
import { IUser } from '../types/IUser';

export const useAuthStore = defineStore({
    id: 'auth',
    state: () => {
        // initialize state from local storage to enable user to stay logged in
        const user = localStorage.getItem('user')
        const usrObj = (user?JSON.parse(user):null) as IUser | null
        return {
            user: usrObj,
            returnUrl: ""
        }
    },
    actions: {
        async login(username : string, password: string) {
            const user = await fetchWrapper.post("/api/Users/authenticate", { username, password });

            // update pinia state
            this.user = user;

            // store user details and jwt in local storage to keep user logged in between page refreshes
            localStorage.setItem('user', JSON.stringify(user));

            // redirect to previous url or default to home page
            router.push(this.returnUrl || '/');
        },
        logout() {
            this.user = null;
            localStorage.removeItem('user');
            router.push('/login');
        }
    }
});

import { RouteLocation, createRouter, createWebHistory } from 'vue-router';

import { useAuthStore } from './stores';
import Login from './views/Login.vue';
import Contact from './views/Contact.vue';
import Templates from './views/Templates.vue';
import EmailForm from './views/EmailForm.vue';

export const router = createRouter({
    history: createWebHistory(),
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Contact },
        { path: '/templates', component: Templates },
        { path: '/message/:slug', component: EmailForm },
        { path: '/login', component: Login }
    ]
});

router.beforeEach(async (to: RouteLocation) => {
    // redirect to login page if not logged in and trying to access a restricted page
    
    const publicPages = ['/login', '/'];
    // http://localhost:5084/message/24A8B9942DCE9A125A7E3A7918BDED84D86A7CFDBD89A88EAA0157E1B8CDA43C
    const isOneTimeToken = to.path.startsWith('/message') && to.path.length==73 // length of message + SHA256
    const authRequired = !publicPages.includes(to.path) && !isOneTimeToken;

    const auth = useAuthStore();
    if (authRequired && !auth.user) {
        auth.returnUrl = to.fullPath;
        return '/login';
    }
});



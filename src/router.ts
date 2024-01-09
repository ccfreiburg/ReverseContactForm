import { RouteLocation, createRouter, createWebHistory } from 'vue-router';
import availableLocales from './main';
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
        { path: '/en', component: Contact },
        { path: '/m/:slug', component: Contact },
        { path: '/en/m/:slug', component: Contact },
        { path: '/templates', component: Templates },
        { path: '/en/templates', component: Templates },
        { path: '/message/:slug', component: EmailForm },
        { path: '/en/message/:slug', component: EmailForm },
        { path: '/login', component: Login },
        { path: '/en/login', component: Login }
    ]
});

router.beforeEach(async (to: RouteLocation) => {
    var to_lang_part =   to.path.substring(1,3);
    var to_compare_part =   to.path.substring(3);
    if (availableLocales && availableLocales.includes(to_lang_part)) {
        to_lang_part = "/" + to_lang_part
    } else {
        to_lang_part = ""
        to_compare_part = to.path
    }
    if (to_compare_part=="") to_compare_part ="/"

    // set the current language for vuex-i18n. note that translation data
    // for the language might need to be loaded first
    const publicPages = ['/login', '/'];
    // http://localhost:5084/message/24A8B9942DCE9A125A7E3A7918BDED84D86A7CFDBD89A88EAA0157E1B8CDA43C
    const isOneTimeToken = to_compare_part.startsWith('/message') && to_compare_part.length==73 // length of message + SHA256
    const authRequired = !publicPages.includes(to_compare_part) && !isOneTimeToken && !to_compare_part.startsWith("/m");
    // redirect to login page if not logged in and trying to access a restricted page

    const auth = useAuthStore();
    if (authRequired && !auth.user) {
        auth.returnUrl = to.fullPath;
        return to_lang_part+'/login';
    }

});



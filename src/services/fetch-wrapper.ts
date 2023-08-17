import { useAuthStore } from '../stores';

export const fetchWrapper = {
    get: request('GET'),
    getraw: request_raw('GET'),
    post: request('POST'),
    postraw: request_raw('POST'),
    put: request('PUT'),
    delete: request('DELETE')
};

function request(method: string) {
    const f = request_raw(method);
    return async (url: string, body?: any) => f(url,body).then(handleResponse);
}

function request_raw(method: string) {
    return async (url: string, body?: any) => {
        const requestOptions = {
            method,
            headers: authHeader(url),
        };
        if (body) {
            requestOptions.headers['Content-Type'] = 'application/json';
            requestOptions['body'] = JSON.stringify(body) as string;
        }
        return fetch(url, requestOptions);
    }
}

// helper functions

function authHeader(url: string) : any {
    // return auth header with jwt if user is logged in and request is to the api url
    const { user } = useAuthStore();
    const isLoggedIn = !!user?.token;
    const isApiUrl = url.startsWith('/api');
    if (isLoggedIn && isApiUrl) {
        return { Authorization: `Bearer ${user.token}` };
    } else {
        return {};
    }
}

function handleResponse(response: any) {
    if (!response.ok) {
        const { user, logout } = useAuthStore();
        if ([401, 403].includes(response.status) && user) {
                // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
            logout();
        }
        const error = response.statusText;
        return Promise.reject(error);
    }
    return response.json();
}

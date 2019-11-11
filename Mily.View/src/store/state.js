// 如果本地缓存里有token，就将token赋值
let objects = {
    defaultToken: '',
    RouterData: '',
    MenuData:''
}
try {
    let token = localStorage.token;
    let router = localStorage.Router;
    let menu = localStorage.Menu;
    if (token) {
        objects.defaultToken = localStorage.token
    };
    if (router) {
        objects.RouterData = JSON.parse(localStorage.Router)
    };
    if (menu) {
        objects.MenuData = JSON.parse(localStorage.Menu)
    }
} catch (e) { }

export default {
    token: objects.defaultToken,
    loadmenus: false,
    RouterData: objects.RouterData,
    MenuData: objects.MenuData
}

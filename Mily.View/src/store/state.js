// 如果本地缓存里有token，就将token赋值
let objects = {
    defaultToken: '',
    RouterData: '',
    MenuData:''
}
try {
    if (localStorage.token) {
        objects.defaultToken = localStorage.token
    };
    if (localStorage.Router) {
        objects.RouterData = JSON.parse(localStorage.Router)
    };
    if (localStorage.Menu) {
        objects.MenuData = JSON.parse(localStorage.Menu)
    }
} catch (e) { }

export default {
    token: objects.defaultToken,
    loadmenus: false,
    RouterData: objects.RouterData,
    MenuData: objects.MenuData
}

import store from '../store/store';
import Routers from '../functions/DynamicMenu';

//检查路由
const RouterCheck = async (next) => {
    if (store.getters.IsLogin == true && store.getters.IsLoadRouter == false) {
        if (await Routers.InitRouter()) 
            next();
    }
    else if (store.getters.IsLogin == true && store.getters.IsLoadRouter == true) {
        Routers.InitCacheRouter();
        next();
    } else next();
}

export default RouterCheck;
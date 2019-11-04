<template>
    <div class="sidebar">
        <el-menu class="sidebar-el-menu" :default-active="onRoutes" :collapse="collapse" background-color="#324157" text-color="#bfcbd9" active-text-color="#20a0ff" unique-opened router>
            <template v-for="(lv1,index) in Menus">
                <!--一级菜单-->
                <template v-if="!lv1.Parent">
                    <el-menu-item :index="lv1.KeyId" :key="lv1.Path">
                        <i :class="lv1.Icon"></i>
                        <span slot="title">{{ lv1.Title }}</span>
                    </el-menu-item>
                </template>
                <template v-else>
                    <!--二级菜单-->
                    <el-submenu :key="lv1.KeyId" :index="lv1.KeyId">
                        <template slot="title">
                            <i :class="lv1.Icon"></i>
                            <span slot="title">{{ lv1.Title }}</span>
                        </template>
                        <!--子菜单-->
                        <template v-for="(lv2,index) in lv1.ChildMenus">
                            <el-submenu :key="lv2.KeyId" :index="lv2.KeyId" v-if="lv2.Parent">
                                <template slot="title">
                                    <i :class="lv2.Icon"></i>
                                    <span slot="title">{{ lv2.Title }}</span>
                                </template>
                                <!--三级菜单-->
                                <el-menu-item v-for="(lv3,index) in lv2.ChildMenus" :index="lv3.Path" :key="lv3.KeyId">
                                    {{lv3.Title}}
                                </el-menu-item>
                            </el-submenu>
                            <!--只有二级菜单无三级菜单-->
                            <el-menu-item :key="lv2.KeyId" :index="lv2.Path" v-else>
                                <span slot="title">{{ lv2.Title }}</span>
                            </el-menu-item>
                        </template>
                    </el-submenu>
                </template>
            </template>
        </el-menu>
    </div>
</template>

<script>
    import bus from './Js/bus';
    import dynamic from '../../router/dynamic'
    import { Menu } from '../../utils/ApiFactory'
    export default {
        data() {
            return {
                collapse: false,
                Menus: [],
                Params: {
                    MapData: { "Key": JSON.parse(this.$store.state.token).RolePermissionId }
                },
                Router: []
            };
        },
        computed: {
            onRoutes() {
                return this.$route.path.replace('/', '');
            }
        },
        created() {
            //初始化菜单
            Menu(this.Params).then(res => {
                this.$options.methods.InitRouter(this, res.ResultData);
                this.Menus = res.ResultData;
            });

            //通过 Event Bus 进行组件间通信，来折叠侧边栏
            bus.$on('collapse', msg => {
                this.collapse = msg;
                bus.$emit('collapse-content', msg);
            });
        },
        methods: {
            /**
             * 初始化路由规则
             * @param options
             * @param data
             */
            InitRouter(options, data) {
                this.InitComponent(data, options.Router);
                options.Router.forEach(item => {
                    let value = item.component;
                    item.component = function component() {
                        import(".." + value);
                    }
                    dynamic.routes[1].children.push(item)
                });
                options.$router.options.routes = dynamic.routes;
                options.$router.addRoutes(dynamic.routes);
            },
            /**
             * 递归路由表
             * @param key
             * @param path
             */
            InitComponent(key, path) {
                key.forEach(item => {
                    if (item.Parent)
                        this.InitComponent(item.ChildMenus, path);
                    else
                        if (item.RouterPath != null)
                            path.push({ "path": "/" + item.Path, "component": item.RouterPath, "meta": { "title": item.Title } });
                });
            }
        }
    };
</script>

<style scoped>
    .sidebar {
        display: block;
        position: absolute;
        left: 0;
        top: 70px;
        bottom: 0;
        overflow-y: scroll;
    }

        .sidebar::-webkit-scrollbar {
            width: 0;
        }

    .sidebar-el-menu:not(.el-menu--collapse) {
        width: 250px;
    }

    .sidebar > ul {
        height: 100%;
    }
</style>

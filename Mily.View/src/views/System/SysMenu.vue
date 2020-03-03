<template>
    <div>
        <div class="crumbs">
            <el-breadcrumb separator="el-icon-arrow-right">
                <el-breadcrumb-item>
                    <i class="el-icon-lx-cascades"></i> 菜单列表
                </el-breadcrumb-item>
            </el-breadcrumb>
        </div>
        <!--内容-->
        <div class="container">
            <!--操作按钮组-->
            <div class="handle-box">
                <el-button type="primary"
                           icon="el-icon-delete"
                           class="handle-del mr5"
                           @click="DeleteSelected">批量删除</el-button>
                <el-button type="primary" icon="el-icon-plus" @click="AddMenu" class="handle-del mr10">新增</el-button>
                <el-select v-model="query.KeyWord.MenuLv" placeholder="菜单级别" class="handle-select mr10">
                    <el-option key="1" label="一级菜单" value="1"></el-option>
                    <el-option key="2" label="二级菜单" value="2"></el-option>
                    <el-option key="3" label="三级菜单" value="3"></el-option>
                </el-select>
                <el-input v-model="query.KeyWord.Title" placeholder="菜单名称" class="handle-input mr10"></el-input>
                <el-button type="primary" icon="el-icon-search" @click="SearchMenu">搜索</el-button>
            </div>

        </div>


    </div>
</template>
<style scoped>
    .handle-box {
        margin-bottom: 20px;
    }

    .handle-select {
        width: 120px;
    }

    .handle-input {
        width: 300px;
        display: inline-block;
    }

    .table {
        width: 100%;
        font-size: 14px;
    }

    .red {
        color: #ff0000;
    }

    .mr10 {
        margin-right: 10px;
    }

    .table-td-thumb {
        display: block;
        margin: auto;
        width: 40px;
        height: 40px;
    }
</style>
<script>
    import { SearchMenuPage } from '../../utils/ApiFactory';
    export default {
        data() {
            return {
                query: {
                    KeyWord: {
                        MenuLv: "",
                        Title: ""
                    },
                    PageIndex: 1,
                    PageSize:10
                }
            }
        },
        methods: {
            SearchMenu() {
                SearchMenuPage(this.query).then(res => {
                    debugger;
                })
            }
        }
    }
</script>
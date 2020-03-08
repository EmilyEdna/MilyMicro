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
                <router-link :to="{name:'MenuAdd'}" style="padding-left:5px" class="mr10">
                    <el-button type="primary" icon="el-icon-plus" @click="AddMenu">新增</el-button>
                </router-link>
                <el-select v-model="query.KeyWord.MenuLv" placeholder="菜单级别" class="handle-select mr10">
                    <el-option key="1" label="一级菜单" value="1"></el-option>
                    <el-option key="2" label="二级菜单" value="2"></el-option>
                    <el-option key="3" label="三级菜单" value="3"></el-option>
                </el-select>
                <el-input v-model="query.KeyWord.Title" placeholder="菜单名称" class="handle-input mr10"></el-input>
                <el-button type="primary" icon="el-icon-search" @click="SearchMenu">搜索</el-button>
            </div>
            <!--表单-->
            <el-table :data="tableData"
                      border
                      class="table"
                      ref="multipleTable"
                      header-cell-class-name="table-header"
                      @selection-change="SelectionChange">
                <el-table-column type="selection" width="55" align="center"></el-table-column>
                <el-table-column label="图标" align="center" width="55">
                    <template slot-scope="scope">
                        <i :class="scope.row.Icon"></i>
                    </template>
                </el-table-column>
                <el-table-column prop="Title" label="菜单名称" align="center"></el-table-column>
                <el-table-column label="菜单级别" align="center">
                    <template slot-scope="scope">
                        <small v-if="scope.row.Lv==1">一级菜单</small>
                        <small v-else-if="scope.row.Lv==2">二级菜单</small>
                        <small v-else>三级菜单</small>
                    </template>
                </el-table-column>
                <!--<el-table-column label="头像(查看大图)" align="center">
                    <template slot-scope="scope">
                        <el-image class="table-td-thumb"
                                  :src="scope.row.thumb"
                                  :preview-src-list="[scope.row.thumb]"></el-image>
                    </template>
                </el-table-column>-->
                <el-table-column prop="Path" label="菜单地址" align="center"></el-table-column>
                <el-table-column prop="RouterPath" label="路由地址" align="center"></el-table-column>
                <el-table-column label="状态" align="center">
                    <template slot-scope="scope">
                        <el-tag :type="scope.row.Deleted?'danger':'success'">{{scope.row.Deleted?'不可用':'可用'}}</el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="Created" label="创建时间" align="center"></el-table-column>
                <el-table-column label="操作" width="180" align="center">
                    <template slot-scope="scope">
                        <el-button type="text"
                                   icon="el-icon-edit"
                                   @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                        <el-button type="text"
                                   icon="el-icon-delete"
                                   class="red"
                                   @click="handleDelete(scope.$index, scope.row)">删除</el-button>
                    </template>
                </el-table-column>
            </el-table>
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

    .mr5 {
        margin-right: 5px;
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
    import { DeleteMenu } from '../../utils/ApiFactory';
    export default {
        data() {
            return {
                query: {
                    KeyWord: {
                        MenuLv: "",
                        Title: ""
                    },
                    PageIndex: 1,
                    PageSize: 10
                },
                multipleSelection: [],
                tableData: []
            }
        },
        mounted() {
            this.SearchMenu();
        },
        methods: {
            SearchMenu() {
                SearchMenuPage(this.query).then(res => {
                    this.tableData = res.ResultData
                });
            },
            AddMenu() { },
            DeleteSelected() {
                let param = { KeyId: this.Linq.from(this.multipleSelection).select(item => item.KeyId).toJoinedString(",") }
                DeleteMenu(param).then(res => {
                    if (res.ResultData >= 0) this.SearchMenu();
                });
            },
            SelectionChange(param) {
                this.multipleSelection = param;
            }
        }
    }
</script>
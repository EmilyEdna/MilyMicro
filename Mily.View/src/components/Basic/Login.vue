<template>
    <div>
        <el-form :model="param.MapData" :rules="rules" ref="login">
            <el-form-item prop="Account">
                <el-input v-model="param.MapData.Account" placeholder="请输入用户名">
                    <i slot="prefix" icon="el-icon-lx-people"></i>
                </el-input>
            </el-form-item>
            <el-form-item prop="PassWord">
                <el-input v-model="param.MapData.PassWord" placeholder="请输入密码">
                    <i slot="prefix" icon="el-icon-lx-lock"></i>
                </el-input>
            </el-form-item>
            <el-button type="primary" @click="submit()">登录</el-button>
        </el-form>
    </div>
</template>
<script>
    import { Login } from '../../utils/ApiFactory'
    import axios from 'axios';
    export default {
        data() {
            return {
                param: {
                    RequestPath: 'System_Login_Main',
                    MapData: {
                        Account: '',
                        PassWord: '',
                    },
                    Hit: 100
                },
                rules: {
                    Account: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
                    PassWord: [{ required: true, message: '请输入密码', trigger: 'blur' }],
                }
            };
        },
        methods: {
            submit() {
                this.$refs.login.validate(v => {
                    Login(this.param).then(res => {
                    });
                });
            },
        }
    }
</script>
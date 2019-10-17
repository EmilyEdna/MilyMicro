<template>
    <div class="login-wrap">
        <div class="ms-login">
            <div class="ms-title">后台管理系统</div>
            <el-form :model="param.MapData" :rules="rules" ref="login" label-width="0px" class="ms-content">
                <el-form-item prop="Account">
                    <el-input v-model="param.MapData.Account" placeholder="请输入用户名">
                        <el-button slot="prepend" icon="el-icon-lx-people"></el-button>
                    </el-input>
                </el-form-item>
                <el-form-item prop="PassWord">
                    <el-input v-model="param.MapData.PassWord" placeholder="请输入密码">
                        <el-button slot="prepend" icon="el-icon-lx-lock"></el-button>
                    </el-input>
                </el-form-item>
                <div class="login-btn">
                    <el-button type="primary" @click="submit()">登录</el-button>
                </div>
            </el-form>
        </div>
    </div>
</template>
<script>
    import { Login } from '../../utils/ApiFactory'
    export default {
        data() {
            return {
                param: {
                    MapData: {
                        Account: '',
                        PassWord: '',
                    }
                },
                rules: {
                    Account: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
                    PassWord: [{ required: true, message: '请输入密码', trigger: 'blur' }],
                }
            };
        },
        //钩子函数
        mounted() {
        },
        methods: {
            submit() {
                this.$refs.login.validate(v => {
                    Login(this.param).then(res => {
                        this.$router.push('/');
                    });
                });
            },
        }
    }
</script>
<style scoped>

    .login-wrap {
        position: relative;
        width: 100%;
        height: 100%;
        background-image: url(../../assets/img/login-bg.jpg);
        background-size: 100%;
    }

    .ms-title {
        width: 100%;
        line-height: 50px;
        text-align: center;
        font-size: 20px;
        color: #fff;
        border-bottom: 1px solid #ddd;
    }

    .ms-login {
        position: absolute;
        left: 50%;
        top: 50%;
        width: 350px;
        margin: -190px 0 0 -175px;
        border-radius: 5px;
        background: rgba(255, 255, 255, 0.3);
        overflow: hidden;
    }

    .ms-content {
        padding: 30px 30px;
    }

    .login-btn {
        text-align: center;
    }

        .login-btn button {
            width: 100%;
            height: 36px;
            margin-bottom: 10px;
        }

    .login-tips {
        font-size: 12px;
        line-height: 30px;
        color: #fff;
    }
</style>
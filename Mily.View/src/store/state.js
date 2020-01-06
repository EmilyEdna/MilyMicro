const state = {
    USER: localStorage.USER ? JSON.parse(localStorage.USER) : null,
    RoleMenu: localStorage.RoleMneu ? JSON.parse(localStorage.RoleMneu) : null
}

export default state;
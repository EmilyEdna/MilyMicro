const state = {
    USER: localStorage.USER ? JSON.parse(localStorage.USER) : null,
    RoleRouter: localStorage.RoleRouter ? JSON.parse(localStorage.RoleRouter) : null
}

export default state;
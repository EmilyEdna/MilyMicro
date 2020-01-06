export const Session = {
    IsLogin: sessionStorage.IsLogin == 'true' ? true : false,
    IsLoadMenu: sessionStorage.IsLoadMenu == 'true' ? true : false,
}

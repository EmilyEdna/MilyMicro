export const Session = {
    IsLogin: sessionStorage.IsLogin == 'true' ? true : false,
    IsLoadRouter: sessionStorage.IsLoadRouter == 'true' ? true : false,
}

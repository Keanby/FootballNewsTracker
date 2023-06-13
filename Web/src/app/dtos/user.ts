export interface User {
  id: number,
  username: number,
  password: number,
}

export interface UserLogin {
  username: number,
  login: number,
  jwtToken: string
}

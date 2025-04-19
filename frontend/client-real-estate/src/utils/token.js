import { jwtDecode } from "jwt-decode";
import { logoutUser } from "../api/auth";
import { renewAuthToken } from "../api/auth";

export const setAccessToken = (token) => {
  localStorage.setItem("accessToken", token);
};

export const getAccessToken = () => {
    return localStorage.getItem("accessToken");
  };
  
export const getRefreshToken = () => {
  return localStorage.getItem("refreshToken");
};

export const setRefreshToken = (token) => {
  localStorage.setItem("refreshToken", token);
};

export const decodeAccessToken = (token) => {
  try {
    const decoded = jwtDecode(token);
    return decoded;
  } catch (error) {
    console.error("Error decoding token:", error);
    return null;
  }
};

export const monitorTokenExpiry = async () => {
  const accessToken = getAccessToken();
  const refreshToken = getRefreshToken();

  if (!accessToken || !refreshToken) {
    console.warn("No tokens found. User might not be logged in.");
    return;
  }

  const decodedAccessToken = decodeAccessToken(accessToken);

  if (!decodedAccessToken || !decodedAccessToken.exp) {
    console.error("Invalid access token. Logging out user.");
    logoutUser();
    return;
  }

  const currentTime = Math.floor(Date.now() / 1000); 
  const timeToExpiry = decodedAccessToken.exp - currentTime;

  if (timeToExpiry <= 60) {
    try {
      const response = await renewAuthToken({ accessToken, refreshToken });
      localStorage.setItem("accessToken", response.token);
      localStorage.setItem("refreshToken", response.refreshToken);
      console.log("Tokens refreshed successfully.");
    } catch (error) {
      console.error("Error refreshing tokens:", error);
      logoutUser();
    }
  }
};

export const isTokenValid = (token) => {
  if (!token) return false;

  const decodedToken = decodeAccessToken(token);
  if (!decodedToken || !decodedToken.exp) return false;

  const currentTime = Math.floor(Date.now() / 1000); 
  return decodedToken.exp > currentTime; 
};

let refreshInterval = null;

export const startTokenRefresh = () => {
  const refreshTokenValue = getRefreshToken();

  if (!refreshTokenValue) {
    console.warn("No refresh token found. User might not be logged in.");
    return;
  }

  
  refreshInterval = setInterval(async () => {
    try {
      const response = await renewAuthToken({
        refreshToken: refreshTokenValue,
      });
      localStorage.setItem("accessToken", response.token);
      localStorage.setItem("refreshToken", response.refreshToken);
      console.log("Access token refreshed successfully.");
    } catch (error) {
      console.error("Error refreshing access token:", error);
      clearInterval(refreshInterval);
      logoutUser();
    }
  }, 4 * 60 * 1000); // 4 minutes in milliseconds
};

export const stopTokenRefresh = () => {
  if (refreshInterval) {
    clearInterval(refreshInterval);
    refreshInterval = null;
  }
};

export const getUserNameFromToken = () => {
  const accessToken = getAccessToken();
  if (!accessToken) {
    console.warn('Access token not found in localStorage.');
    return null;
  }

  const decodedToken = decodeAccessToken(accessToken);
  if (decodedToken && decodedToken.name) {
    return decodedToken.name;
  } else {
    console.error("Failed to decode access token or extract user name.");
    return null;
  }
};

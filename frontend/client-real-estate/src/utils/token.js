import { jwtDecode } from "jwt-decode";
import { logoutUser } from "../api/auth";
import { renewAuthToken } from "../api/auth"; // Assuming you have a function to renew the token

// Function to set the access token in localStorage
export const setAccessToken = (token) => {
  localStorage.setItem("accessToken", token);
};

// Function to get the access token from localStorage
export const getAccessToken = () => {
    return localStorage.getItem("accessToken");
  };
  

// Function to get the refresh token from localStorage
export const getRefreshToken = () => {
  return localStorage.getItem("refreshToken");
};

// Function to set the refresh token in localStorage
export const setRefreshToken = (token) => {
  localStorage.setItem("refreshToken", token);
};

// Function to decode the access token and check expiry
export const decodeAccessToken = (token) => {
  try {
    const decoded = jwtDecode(token);
    return decoded;
  } catch (error) {
    console.error("Error decoding token:", error);
    return null;
  }
};

// Function to refresh tokens a minute before expiry
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

  const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds
  const timeToExpiry = decodedAccessToken.exp - currentTime;

  if (timeToExpiry <= 60) {
    // If less than or equal to 1 minute
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

  const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds
  return decodedToken.exp > currentTime; // Token is valid if expiration time is in the future
};

let refreshInterval = null;

export const startTokenRefresh = () => {
  const refreshTokenValue = getRefreshToken();

  if (!refreshTokenValue) {
    console.warn("No refresh token found. User might not be logged in.");
    return;
  }

  // Set interval to refresh the access token every 4 minutes
  refreshInterval = setInterval(async () => {
    try {
      const response = await renewAuthToken({ refreshToken: refreshTokenValue });
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

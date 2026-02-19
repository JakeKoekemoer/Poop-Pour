import { getApiClient } from '../api/clientFactory';
import type { ApiResponse, ApiError } from '../types';
import { Client, SwaggerResponse, ApiException } from '@/api/api-client';

export abstract class BaseService {
  protected get client(): Client {
    return getApiClient();
  }

  protected async execute<T>(
    apiCall: () => Promise<SwaggerResponse<T>>
  ): Promise<ApiResponse<T>> {
    try {
      const response = await apiCall();
      return { success: true, data: response.result };
    } catch (error: unknown) {
      return { success: false, error: this.handleError(error) };
    }
  }

  protected async executeVoid(
    apiCall: () => Promise<SwaggerResponse<void>>
  ): Promise<ApiResponse<void>> {
    try {
      await apiCall();
      return { success: true, data: undefined };
    } catch (error: unknown) {
      return { success: false, error: this.handleError(error) };
    }
  }

  private handleError(error: unknown): ApiError {
    if (error instanceof ApiException) {
      return {
        message: this.getErrorMessage(error),
        statusCode: error.status,
        validationErrors: this.parseValidationErrors(error.response),
        type: this.getErrorType(error.status),
      };
    }

    if (error instanceof Error && error.message.includes('token has expired')) {
      return {
        message: 'Your session has expired. Please log in again.',
        type: 'auth',
      };
    }

    if (error instanceof TypeError && error.message.includes('fetch')) {
      return {
        message: 'Network error. Please check your connection and try again.',
        type: 'network',
      };
    }

    if (error instanceof Error) {
      return {
        message: error.message,
        type: 'unknown',
      };
    }

    return {
      message: 'An unexpected error occurred. Please try again.',
      type: 'unknown',
    };
  }

  private getErrorMessage(exception: ApiException): string {
    try {
      const parsed = JSON.parse(exception.response);
      
      if (parsed.title) {
        return parsed.title;
      }
      if (parsed.message) {
        return parsed.message;
      }
      if (parsed.error) {
        return parsed.error;
      }
      
      if (parsed.errors && typeof parsed.errors === 'object') {
        return 'Validation failed. Please check your input.';
      }
    } catch {
    }

    return exception.message || 'An error occurred';
  }

  private getErrorType(statusCode: number): ApiError['type'] {
    if (statusCode === 401 || statusCode === 403) {
      return 'auth';
    }
    if (statusCode === 400 || statusCode === 422) {
      return 'validation';
    }
    if (statusCode >= 500) {
      return 'server';
    }
    if (statusCode === 0) {
      return 'network';
    }
    return 'unknown';
  }

  private parseValidationErrors(
    response: string
  ): Record<string, string[]> | undefined {
    try {
      const parsed = JSON.parse(response);
      
      if (parsed.errors && typeof parsed.errors === 'object') {
        const errors: Record<string, string[]> = {};
        for (const [key, value] of Object.entries(parsed.errors)) {
          if (Array.isArray(value)) {
            errors[key] = value.filter(v => typeof v === 'string');
          } else if (typeof value === 'string') {
            errors[key] = [value];
          }
        }
        return Object.keys(errors).length > 0 ? errors : undefined;
      }
    } catch {
    }
    return undefined;
  }
}

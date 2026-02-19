import * as z from "zod";

export const registerSchema = z.object({
  email: z.string().email("Invalid email address"),
  userName: z
    .string()
    .min(3, "Username must be at least 3 characters")
    .max(50, "Username must be less than 50 characters"),
  password: z.string().min(8, "Password must be at least 8 characters"),
  firstName: z.string().optional(),
  lastName: z.string().optional(),
});

export type RegisterFormValues = z.infer<typeof registerSchema>;

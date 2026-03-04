import * as z from 'zod'

export const addUserSchema = z.object({
  email: z.string().email('Invalid email address'),
  userName: z
    .string()
    .min(3, 'Username must be at least 3 characters')
    .max(50, 'Username must be less than 50 characters'),
  password: z.string().min(8, 'Password must be at least 8 characters'),
  firstName: z.string().optional(),
  lastName: z.string().optional(),
  role: z.string().min(1, 'Role is required'),
})

export type AddUserFormValues = z.infer<typeof addUserSchema>

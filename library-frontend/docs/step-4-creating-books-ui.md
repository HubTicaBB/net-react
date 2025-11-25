# Step 4: Creating the Books UI components

## What I'll do:

- Create a Books page to display all books in a table or grid
- Create a BookCard/BookList component for displaying books
- Add a Create Book form (Librarian only)
- Add Edit/Delete functionality (Librarian only)
- Show role-based UI (hide create/edit/delete for Members)
- Add loading and error states

## Why this way:

- Role-based UI: only show actions the user can perform
- Reusable components: BookCard can be used in multiple places
- Clear separation: list view, create form, edit form as separate components
- User feedback: loading states and error messages

## What I'll create:

- src/pages/Books.tsx - Main books page with list
- src/components/BookCard.tsx - Component to display a single book
- src/components/BookForm.tsx - Reusable form for create/edit
- src/components/BookModal.tsx - Modal for create/edit (optional, or inline form)
- Update routing in App.tsx to include /books route

### Features:

- Display all books in a table/grid
- Create button (Librarian only)
- Edit button (Librarian only)
- Delete button (Librarian only)
- View book details
- Loading states
- Error handling
- Proceed with Step 4?

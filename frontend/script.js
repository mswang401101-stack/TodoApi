const API_URL = 'http://localhost:5000/todoitems'; // 假設後端服務運行在 localhost:5000

const todoForm = document.getElementById('todo-form');
const todoInput = document.getElementById('todo-input');
const todoList = document.getElementById('todo-list');

// 獲取所有待辦事項並渲染
async function fetchTodos() {
    try {
        const response = await fetch(API_URL);
        const todos = await response.json();
        todoList.innerHTML = ''; // 清空現有列表
        todos.forEach(todo => {
            const li = document.createElement('li');
            li.className = todo.isCompleted ? 'completed' : '';
            li.dataset.id = todo.id;
            li.innerHTML = `
                <input type="checkbox" ${todo.isCompleted ? 'checked' : ''}>
                <span>${todo.title}</span>
                <button>刪除</button>
            `;
            todoList.appendChild(li);
        });
    } catch (error) {
        console.error('獲取待辦事項失敗:', error);
        alert('無法連接到後端服務，請確保後端服務正在運行。');
    }
}

// 新增待辦事項
async function addTodo(e) {
    e.preventDefault();
    const title = todoInput.value.trim();
    if (!title) return;

    try {
        await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ title, isCompleted: false }),
        });
        todoInput.value = '';
        fetchTodos(); // 重新載入列表
    } catch (error) {
        console.error('新增待辦事項失敗:', error);
        alert('新增待辦事項失敗，請稍後再試。');
    }
}

// 切換待辦事項完成狀態
async function toggleTodoCompletion(id, isCompleted) {
    try {
        await fetch(`${API_URL}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ isCompleted: !isCompleted }),
        });
        fetchTodos(); // 重新載入列表
    } catch (error) {
        console.error('更新待辦事項失敗:', error);
        alert('更新待辦事項狀態失敗，請稍後再試。');
    }
}

// 刪除待辦事項
async function deleteTodo(id) {
    try {
        await fetch(`${API_URL}/${id}`, {
            method: 'DELETE',
        });
        fetchTodos(); // 重新載入列表
    } catch (error) {
        console.error('刪除待辦事項失敗:', error);
        alert('刪除待辦事項失敗，請稍後再試。');
    }
}

// 事件監聽器
todoForm.addEventListener('submit', addTodo);

todoList.addEventListener('click', (e) => {
    const li = e.target.closest('li');
    if (!li) return;

    const id = li.dataset.id;
    const isCompleted = li.classList.contains('completed');

    if (e.target.type === 'checkbox') {
        toggleTodoCompletion(id, isCompleted);
    } else if (e.target.tagName === 'SPAN') {
        toggleTodoCompletion(id, isCompleted); // 點擊文字也切換完成狀態
    } else if (e.target.tagName === 'BUTTON') {
        deleteTodo(id);
    }
});

// 頁面載入時獲取待辦事項
document.addEventListener('DOMContentLoaded', fetchTodos);

import './App.css'
import { useState } from "react"
import { Squares2X2Icon, ChartBarIcon, PlusIcon, SunIcon, MoonIcon } from '@heroicons/react/24/outline'


type Task = {
  id: number
  title: string
  description: string
  status: "To Do" | "In Progress" | "Completed"
  createdAt: string
  dueDate: string
  completedAt?: string | null
  isActive: boolean
  hoursWorked: number
}

function App() {
  const [tasks, setTasks] = useState<Task[]>([
    {
      id: 11,
      title: "Fix authentication bug",
      description: "Users reporting issues with password reset flow",
      status: "To Do",
      createdAt: "2026-04-11T12:00:00Z",
      dueDate: "2026-04-18T12:00:00Z",
      completedAt: null,
      isActive: true,
      hoursWorked: 0,
    },
  ])

   const [selectedTask, setSelectedTask] = useState<Task | null>(null)
  const [isNewTask, setIsNewTask] = useState(false)
  const [searchQuery, setSearchQuery] = useState("")
  const [filterStatus, setFilterStatus] = useState<"All" | Task["status"]>("All")
  const [darkMode, setDarkMode] = useState(false)

  const updateTaskStatus = (id: number, newStatus: Task["status"]) => {
    setTasks(prev =>
      prev.map(t =>
        t.id === id
          ? {
              ...t,
              status: newStatus,
              completedAt: newStatus === "Completed" ? new Date().toISOString() : t.completedAt,
            }
          : t
      )
    )
  }

  const addTask = (task: Task) => {
    setTasks(prev => [...prev, task])
  }
    const filteredTasks = tasks.filter(t =>
    (filterStatus === "All" || t.status === filterStatus) &&
    t.title.toLowerCase().includes(searchQuery.toLowerCase())
  )
  const deleteTask = (id: number) => {
  setTasks(prev => prev.filter(t => t.id !== id))
}

  return (
    <div className={`${darkMode ? "bg-gray-900 text-gray-100" : "bg-gradient-to-r from-indigo-50 via-purple-50 to-pink-50"} flex min-h-screen`}>
      {/* Sidebar */}
      <aside className={`${darkMode ? "bg-gray-800" : "bg-purple-700"} w-64 text-white flex flex-col p-6`}>
        <h1 className="text-2xl font-bold mb-8">ProdDash</h1>
        <nav className="flex flex-col gap-4">
          <a href="#" className="hover:bg-purple-600 p-2 rounded flex items-center gap-2">
            <Squares2X2Icon className="h-5 w-5" /> Tasks
          </a>
          <a href="#" className="hover:bg-purple-600 p-2 rounded flex items-center gap-2">
            <ChartBarIcon className="h-5 w-5" /> Analytics
          </a>
        </nav>
        <button
          className="mt-auto bg-green-500 hover:bg-green-600 text-white py-2 rounded flex items-center gap-2"
          onClick={() => {
            setIsNewTask(true)
            setSelectedTask({
              id: Date.now(),
              title: "",
              description: "",
              status: "To Do",
              createdAt: new Date().toISOString(),
              dueDate: new Date().toISOString(),
              completedAt: null,
              isActive: true,
              hoursWorked: 0,
            })
          }}
        >
          <PlusIcon className="h-5 w-5" /> New Task
        </button>
        <button
          className="mt-4 flex items-center gap-2 text-sm hover:underline"
          onClick={() => setDarkMode(!darkMode)}
        >
          {darkMode ? <SunIcon className="h-5 w-5" /> : <MoonIcon className="h-5 w-5" />}
          {darkMode ? "Light Mode" : "Dark Mode"}
        </button>
      </aside>

      {/* Main Content */}
      <main className="flex-1 p-6">
        {/* Search + Filters */}
        <div className="flex gap-4 mb-6">
          <input
            type="text"
            placeholder="Search tasks..."
            value={searchQuery}
            onChange={e => setSearchQuery(e.target.value)}
            className="flex-1 border rounded px-3 py-2"
          />
          <select
            value={filterStatus}
            onChange={e => setFilterStatus(e.target.value as "All" | Task["status"])}
            className="border rounded px-3 py-2"
          >
            <option>All</option>
            <option>To Do</option>
            <option>In Progress</option>
            <option>Completed</option>
          </select>
        </div>

        {/* Dashboard Stats */}
        <div className="grid grid-cols-4 gap-6 mb-6">
          <StatCard label="Total Tasks" value={tasks.length} color="text-gray-800" />
          <StatCard label="Completed" value={tasks.filter(t => t.status === "Completed").length} color="text-green-600" />
          <StatCard label="In Progress" value={tasks.filter(t => t.status === "In Progress").length} color="text-blue-600" />
          <StatCard label="Overdue" value={tasks.filter(t => new Date(t.dueDate) < new Date() && t.status !== "Completed").length} color="text-red-600" />
        </div>

        {/* Task Board */}
        <div className="grid grid-cols-3 gap-6">
          {["To Do", "In Progress", "Completed"].map(col => (
            <section key={col} className={`${darkMode ? "bg-gray-800 text-gray-100" : "bg-white"} rounded shadow p-4`}>
              <h2 className="text-lg font-semibold mb-4">{col}</h2>
              <div className="space-y-4">
                {filteredTasks.filter(t => t.status === col).map(t => (
                  <TaskCard key={t.id} task={t} onClick={setSelectedTask} darkMode={darkMode} />
                ))}
              </div>
            </section>
          ))}
        </div>
      </main>

      {selectedTask && (
        <TaskModal
          task={selectedTask}
          onClose={() => {
            setSelectedTask(null)
            setIsNewTask(false)
          }}
          onStatusChange={updateTaskStatus}
          onSave={task => {
            if (isNewTask) {
              addTask(task)
            }
            setSelectedTask(null)
            setIsNewTask(false)
          }}
          onDelete={deleteTask}  
          isNew={isNewTask}
          darkMode={darkMode}
        />
      )}
    </div>
  )



function TaskCard({ task, onClick, darkMode }: { task: Task; onClick: (t: Task) => void; darkMode: boolean }) {
  return (
    <div
      className={`${darkMode ? "bg-gray-700 text-gray-100" : "bg-white"} rounded-lg shadow-md p-4 hover:shadow-lg transition cursor-pointer`}
      onClick={() => onClick(task)}
    >
      <h3 className="font-semibold text-indigo-500">{task.title}</h3>
      <p className="text-sm">{task.description}</p>
      <div className="flex justify-between text-xs mt-2">
        <span>📅 {new Date(task.dueDate).toLocaleDateString()}</span>
        {task.hoursWorked > 0 && <span>⏱ {task.hoursWorked}h</span>}
      </div>
    </div>
  )
}

function StatCard({ label, value, color }: { label: string; value: number; color: string }) {
  return (
    <div className="bg-white shadow rounded p-4">
      <h3 className="text-gray-500">{label}</h3>
      <p className={`text-2xl font-bold ${color}`}>{value}</p>
    </div>
  )
}
function TaskModal({
  task,
  onClose,
  onStatusChange,
  onSave,
  onDelete,   // 👈 new prop
  isNew,
  darkMode,
}: {
  task: Task
  onClose: () => void
  onStatusChange: (id: number, newStatus: Task["status"]) => void
  onSave: (task: Task) => void
  onDelete: (id: number) => void   // 👈 new type
  isNew: boolean
  darkMode: boolean
}) {
  const [localTask, setLocalTask] = useState<Task>(task)

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
      <div className={`${darkMode ? "bg-gray-800 text-gray-100" : "bg-white"} rounded-lg shadow-lg p-6 w-[400px]`}>
        <h2 className="text-xl font-bold mb-4">{isNew ? "New Task" : "Edit Task"}</h2>
        <form
          className="space-y-4"
          onSubmit={e => {
            e.preventDefault()
            onSave(localTask)
          }}
        >
          {/* Title */}
          <div>
            <label className="block text-sm font-medium">Title</label>
            <input
              type="text"
              value={localTask.title}
              onChange={e => setLocalTask({ ...localTask, title: e.target.value })}
              className="w-full border rounded px-2 py-1"
            />
          </div>

          {/* Description */}
          <div>
            <label className="block text-sm font-medium">Description</label>
            <textarea
              value={localTask.description}
              onChange={e => setLocalTask({ ...localTask, description: e.target.value })}
              className="w-full border rounded px-2 py-1"
            />
          </div>

          {/* Status */}
          <div>
            <label className="block text-sm font-medium">Status</label>
            <select
              value={localTask.status}
              onChange={e => {
                const newStatus = e.target.value as Task["status"]
                setLocalTask({ ...localTask, status: newStatus })
                onStatusChange(localTask.id, newStatus)
              }}
              className="w-full border rounded px-2 py-1"
            >
              <option>To Do</option>
              <option>In Progress</option>
              <option>Completed</option>
            </select>
          </div>

          {/* Due Date */}
          <div>
            <label className="block text-sm font-medium">Due Date</label>
            <input
              type="date"
              value={localTask.dueDate.split("T")[0]}
              onChange={e => setLocalTask({ ...localTask, dueDate: e.target.value })}
              className="w-full border rounded px-2 py-1"
            />
          </div>

          {/* Hours Worked */}
          <div>
            <label className="block text-sm font-medium">Hours Worked</label>
            <input
              type="number"
              value={localTask.hoursWorked}
              onChange={e => setLocalTask({ ...localTask, hoursWorked: Number(e.target.value) })}
              className="w-full border rounded px-2 py-1"
            />
          </div>

          {/* Buttons */}
          <div className="flex justify-end gap-2">
            <button type="button" onClick={onClose} className="px-3 py-1 border rounded">
              Cancel
            </button>
            {!isNew && (
              <button
                type="button"
                onClick={() => {
                  onDelete(task.id)
                  onClose()
                }}
                className="px-3 py-1 bg-red-600 text-white rounded"
              >
                Delete
              </button>
            )}
            <button type="submit" className="px-3 py-1 bg-purple-600 text-white rounded">
              {isNew ? "Add Task" : "Save Changes"}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
}

export default App

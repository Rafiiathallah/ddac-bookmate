"use client";

import { motion } from "framer-motion";
import { Search, Book } from "lucide-react";
import Link from "next/link";

// Sample books data
const SAMPLE_BOOKS = [
  {
    title: "The Midnight Library",
    author: "Matt Haig",
    coverColor: "bg-emerald-100",
  },
  {
    title: "Project Hail Mary",
    author: "Andy Weir",
    coverColor: "bg-blue-100",
  },
  {
    title: "Dune",
    author: "Frank Herbert",
    coverColor: "bg-amber-100",
  },
  {
    title: "The Song of Achilles",
    author: "Madeline Miller",
    coverColor: "bg-rose-100",
  },
  {
    title: "Klara and the Sun",
    author: "Kazuo Ishiguro",
    coverColor: "bg-violet-100",
  },
  {
    title: "The Seven Husbands of Evelyn Hugo",
    author: "Taylor Jenkins Reid",
    coverColor: "bg-teal-100",
  },
  {
    title: "Tomorrow, and Tomorrow, and Tomorrow",
    author: "Gabrielle Zevin",
    coverColor: "bg-indigo-100",
  },
  {
    title: "A Court of Thorns and Roses",
    author: "Sarah J. Maas",
    coverColor: "bg-pink-100",
  },
];

export default function ExplorePage() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      {/* Navigation */}
      <nav className="sticky top-0 border-b bg-white/50 backdrop-blur-sm">
        <div className="mx-auto max-w-7xl px-6 py-4">
          <div className="flex items-center justify-between">
            <Link
              href="/"
              className="flex items-center space-x-2 hover:opacity-80 transition-opacity"
            >
              <Book className="h-6 w-6 text-blue-600" />
              <span className="text-xl font-semibold text-blue-600">
                Bookmate
              </span>
            </Link>
            <div className="relative w-full max-w-md mx-4">
              <Search className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-gray-400" />
              <input
                type="text"
                placeholder="Search books..."
                className="w-full rounded-full border border-gray-200 bg-white/50 py-2 pl-10 pr-4 text-sm text-gray-900 placeholder-gray-400 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500 transition-colors"
              />
            </div>
            <button className="rounded-full bg-blue-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-700 transition-colors">
              Sign In
            </button>
          </div>
        </div>
      </nav>

      {/* Main Content */}
      <main className="mx-auto max-w-7xl px-6 py-8">
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
          {SAMPLE_BOOKS.map((book, i) => (
            <motion.div
              key={i}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.3, delay: i * 0.1 }}
              className="group cursor-pointer"
            >
              <div className="relative overflow-hidden rounded-lg bg-white shadow-md hover:shadow-xl transition-all duration-300">
                <div
                  className={`aspect-[3/4] ${book.coverColor} group-hover:scale-105 transition-transform duration-300`}
                />
                <div className="absolute inset-0 bg-gradient-to-t from-black/20 to-transparent opacity-0 group-hover:opacity-100 transition-opacity" />
                <div className="p-4">
                  <h3 className="font-semibold text-gray-900 line-clamp-1">
                    {book.title}
                  </h3>
                  <p className="mt-1 text-sm text-gray-500">{book.author}</p>
                </div>
              </div>
            </motion.div>
          ))}
        </div>
      </main>
    </div>
  );
}

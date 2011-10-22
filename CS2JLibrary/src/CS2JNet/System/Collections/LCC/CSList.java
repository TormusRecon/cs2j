/*
   Copyright 2010,2011 Kevin Glynn (kevin.glynn@twigletsoftware.com)

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.

   Author(s):

   Kevin Glynn (kevin.glynn@twigletsoftware.com)
*/

package CS2JNet.System.Collections.LCC;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;

import CS2JNet.JavaSupport.Collections.Generic.LCC.EnumeratorSupport;
import CS2JNet.System.ArgumentException;
import CS2JNet.System.ArgumentNullException;
import CS2JNet.System.ArgumentOutOfRangeException;

/**
 * @author keving
 *
 */
public class CSList<T> implements ICollection<T>, IEnumerable<T>, Collection<T> {
	
	private List<T> myList = null;

	public CSList() {
		myList = new ArrayList<T>();
	}
	
	public CSList(int size) {
		myList = new ArrayList<T>(size);
	}
	
	public CSList(IEnumerable<T> coll) {
		myList = new ArrayList<T>();
		for (T el : coll) {
			myList.add(el);
		}
	}
	
	public CSList(T[] coll) {
		myList = new ArrayList<T>(coll.length);
		for (T el : coll) {
			myList.add(el);
		}
	}
	
	@Override
	public Iterator<T> iterator() {
		return myList.iterator();
	}

	@Override
	public boolean add(T arg0) {
		return myList.add(arg0);
	}

	@Override
	public boolean addAll(Collection<? extends T> arg0) {
		return myList.addAll(arg0);
	}

	@Override
	public void clear() {
		myList.clear();
		
	}

	@Override
	public boolean contains(Object arg0) {
		return myList.contains(arg0);
	}

	@Override
	public boolean containsAll(Collection<?> arg0) {
		return myList.containsAll(arg0);
	}

	@Override
	public boolean isEmpty() {
		return myList.isEmpty();
	}

	@Override
	public boolean remove(Object arg0) {
		return myList.remove(arg0);
	}

	@Override
	public boolean removeAll(Collection<?> arg0) {
		return myList.removeAll(arg0);
	}

	@Override
	public boolean retainAll(Collection<?> arg0) {
		return myList.retainAll(arg0);
	}

	@Override
	public int size() {
		return myList.size();
	}

	@Override
	public Object[] toArray() {
		return myList.toArray();
	}

	@Override
	public <S> S[] toArray(S[] arg0) {
		return myList.toArray(arg0);
	}

	@Override
	public boolean Contains(T x) throws Exception {
		return myList.contains(x);
	}

	@Override
	public void Add(T x) throws Exception {
		myList.add(x);
	}

	@Override
	public boolean Remove(T x) throws Exception {
		return myList.remove(x);
	}

	@Override
	public void Clear() throws Exception {
		myList.clear();
	}

	public IEnumerator<T> GetEnumerator() throws Exception {
		return EnumeratorSupport.mk(myList.iterator());
	}

	public void CopyTo(T[] arr, int i) throws Exception {
		if (arr == null)
			throw new ArgumentNullException();
		if (i < 0)
			throw new ArgumentOutOfRangeException();
		if (i + arr.length < myList.size())
			throw new ArgumentException();
		for (int idx = 0; idx < arr.length; idx++){
			arr[idx+i] = myList.get(idx);
		}
	}

	@Override
	public IEnumerator<T> getEnumerator() throws Exception {
		return GetEnumerator();
	}

	@Override
	public void copyTo(T[] arr, int i) throws Exception {
		CopyTo(arr, i);
	}

}
